#!/bin/bash
@echo off

echo Deploying Executive...
scp pi@192.168.1.160:/home/pi/puppet/appsettings.json ./
pushd Puppet.Executive
dotnet publish -r linux-arm -c Release
rm ./bin/Release/netcoreapp2.2/linux-arm/publish/Puppet.Executive.runtimeconfig.json
echo '{ "runtimeOptions": { "configProperties": { "System.Globalization.Invariant": true } } }' >> ./bin/Release/netcoreapp2.2/linux-arm/publish/Puppet.Executive.runtimeconfig.json
ssh pi@192.168.1.160 pkill Puppet.Executive
ssh pi@192.168.1.160 rm /home/pi/puppet/*
scp ./bin/Release/netcoreapp2.2/linux-arm/publish/* pi@192.168.1.160:/home/pi/puppet
popd 
scp ./appsettings.json pi@192.168.1.160:/home/pi/puppet/
rm appsettings.json 
ssh pi@192.168.1.160 chmod 755 /home/pi/puppet/Puppet.Executive

echo Deploying Automation Handlers...
pushd Puppet.Automation
dotnet publish -r linux-arm -c Release
scp ./bin/Release/netcoreapp2.2/linux-arm/publish/Puppet.Automation.* pi@192.168.1.160:/home/pi/puppet
scp ./devicemap.json pi@192.168.1.160:/home/pi/puppet
popd 

echo Rebooting!
ssh pi@192.168.1.160 sudo reboot