#!/bin/bash
echo "Welcome to the club buddy"
ls -l
cd news-server
ls -l
dotnet publish news-server.sln
cd news-server/bin/Debug/netcoreapp3.1
ls -l
scp -rp news-server/news-server/bin/Debug/netcoreapp3.1 root@$server:/home/BackNews/
echo "END!!!"
