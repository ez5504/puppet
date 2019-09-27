using Puppet.Common.Automation;
using Puppet.Common.Devices;
using Puppet.Common.Events;
using Puppet.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Puppet.Common.StateManagement;

namespace Puppet.Executive.Automation
{
    public class AutomationFactory
    {
        private const string _automationAssembly = "Puppet.Automation.dll";

        private static IMemoryCache MemoryCache;

        public static IServiceProvider ServiceProvider;

        static AutomationFactory() 
        {
            MemoryCache = new MemoryCache(new MemoryCacheOptions());
            ServiceProvider = new ServiceCollection()
                .AddMemoryCache()
                .AddSingleton<IWeatherData, WeatherData>()
                .BuildServiceProvider();
        }

        /// <summary>
        /// Figures out the appropriate implementation of IAutomation based on the data in the event and returns it.
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="hub"></param>
        /// <returns>An IEnumerable&lt;IAutomation&gt; containing the automations to be run for this event.</returns>
        public static IEnumerable<IAutomation> GetAutomations(HubEvent evt, HomeAutomationPlatform hub)
        {
            /*
             *  Get the types from the assembly
             *      where the type implements IAutomation and
             *          the type has trigger attributes
             *              where the trigger attribute names a mapped device that matches the device that caused the event
             *                  and the attribute also names a Capability that matches the device that caused the event
             *          and the count of the matching trigger attributes is greater than 0
             */

            
            Dictionary<string, List<Type>> assemblies = MemoryCache.GetOrCreate("Assemblies", entry => 
            {
                Dictionary<string, List<Type>> automationDictionary = new Dictionary<string, List<Type>>();
                
                var temp = Assembly.LoadFrom(_automationAssembly).GetTypes()
                        .Where(t => typeof(IAutomation).IsAssignableFrom(t));
                foreach(var type in temp)
                {
                    var keys = type.GetCustomAttributes<TriggerDeviceAttribute>().Select(t => $"{hub.LookupDeviceId(t.DeviceMappedName)}|{t.Capability.ToString().ToLower()}");
                    foreach(var key in keys)
                    {
                        if(automationDictionary.ContainsKey(key))
                        {
                            automationDictionary[key].Add(type);
                        }
                        else 
                        {
                            automationDictionary.Add(key, new List<Type> { type });
                        }
                    }
                }
                return automationDictionary;
            });

            foreach (Type automation in assemblies[$"{evt.DeviceId}|{evt.Name}"])
            {
                var thing = Activator.CreateInstance(automation, new Object[] { hub, evt });
                if (thing is IAutomation automationSource)
                    yield return automationSource;
            }
        }
    }
}
