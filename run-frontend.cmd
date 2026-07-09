@echo off
set PATH=C:\Program Files\nodejs;%PATH%
cd /d C:\Users\ardak\source\repos\ECommerceApi\ecommerce-frontend
"C:\Program Files\nodejs\npm.cmd" run dev -- --host 0.0.0.0
