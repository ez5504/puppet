#!/bin/bash

echo Deploying Executive...
scp automation@automation:/home/automation/puppet/appsettings.json ./
pushd Puppet.Executive
dotnet publish -r linux-arm -c Release
rm ./bin/Release/netcoreapp2.2/linux-arm/publish/Puppet.Executive.runtimeconfig.json
echo '{ "runtimeOptions": { "configProperties": { "System.Globalization.Invariant": true } } }' >> ./bin/Release/netcoreapp2.2/linux-arm/publish/Puppet.Executive.runtimeconfig.json
ssh automation@automation pkill Puppet.Executive
ssh automation@automation rm /home/automation/puppet/*
scp ./bin/Release/netcoreapp2.2/linux-arm/publish/* automation@automation:/home/automation/puppet
popd 
scp ./appsettings.json automation@automation:/home/automation/puppet/
rm appsettings.json 
ssh automation@automation chmod 755 /home/automation/puppet/Puppet.Executive

echo Deploying Automation Handlers...
pushd Puppet.Automation
dotnet publish -r linux-arm -c Release
scp ./bin/Release/netcoreapp2.2/linux-arm/publish/Puppet.Automation.* automation@automation:/home/automation/puppet
scp ./devicemap.json automation@automation:/home/automation/puppet
popd 

echo Rebooting!
ssh automation@automation sudo reboot