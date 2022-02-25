# Sniffo

## Introduction

This is a simple network sniffer for layers 3 and 4 of the TCP/IP chain of protocols.  
Currently the application sniffes the following protocols:

- [X] IPv4
- [X] TCP
- [X] ICMP

The application currently only can be used on a Windows SO, UNIX-like is not implemented yet.

## Setup

You only need to have in your machine to run this app installed .NET 6, there's no other dependencies to run it. Only need to make sure your user can access the sockets on your machine, may some firewall block the socket port.

> Note: If you are gonna run this application by Visual Studio on Windows, run the Visual Studio as an Administrator for the application can access the sockets.

## Next Steps

Here is the future implementations for this project:

- [ ] IPv6
- [ ] UNIX-like SO support
- [ ] Analytic vision of the sniffed packages
- [ ] e2e Tests
