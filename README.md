# gtSportBE
This is the backend for GTEAM a GT Sport Pitwall and Telemetry Web App

## Purpose
This project takes inspiration from ATLAS (mclaren applied technolgies) and telemetry software like Motec and Track Titan. The aim was to create a web app with functionality between the last two. The project acts as both an experiment with c# and a test of my react and next js understanding. 

It is meant to act as a proof of concept for a semi-pro sim racer or sim racing team. It offers tools for a sim racing team and for individuals looking to improve.


## Table of Content
* [Tech Stack](#Tech)
* [Features](#Features)
* [Run](#Run)
* [Credit](#Credit)
* [License](#License)


## Tech
### Technology Used

@Microsoft.AspNetCore.SignalR - Perhaps the most important part of the application, signalr allows for the connection between the front and backend via a websocket. This is necessary beacuse of the nature of the data. Given there is a constant stream of data a websocket made the most sense for perfomances sake.

@.Net - Allows for cross OS devlopment and access to frameworks such as ASP.NET for web development.

@MongoDB.Driver - Used to connect to mongodb cluster

### Languages 

C# - Selected to align with F1 industry standard. As well as resource provided by NENKAI available in C#.

## How It Works

- NENKAI's simulator interface provides a connection between instance and running game, by providing args of game type and IP address of console.
  
- Task triggered on recieval of a packet from interface that aggregates packets into a larger packet. This larger packet is then averaged and every 0.1 seconds a packet is sent via the signalR hub to the frontend endpoint, once it is established. Upon starting the backend the most recent video and challenge content is checked and if it does not align with the current date then a request to the youtube api is made and the data is updated and similarly the challenge data is updated. The data recieved comes via UDP and is hence a lossy process, this does feed through into the front end, however for the most part this is not noticable.

- Packet aggregation is done given that packets arrive over 6-8 times per 0.1 second, since this many updates would hurt the performance of the front end web app.

## Run

- This project can be run without the frontend, NENKAI documents various ways to use it. Currently these only work locally. You will also need a playstation console and copy of the game GT Sport or another GT game in order to see the full functionality.

  
## Credit

- GT data unpacker and other utils - NENKAI @ ddm999 Daniel Lee, PDTools, 2023, https://github.com/Nenkai/PDTools

## Licence

Copyright (c) 2023 Max Byng-Maddick

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

