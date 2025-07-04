# TvHeadendRecordControlCli
Improved version for TvHeadend recording control

TvHeadendRecordControlCli

Command line program for controlling TvHeadend and adding and deleting TV recordings. Serves as a link between TV Browser and TvHeadend. See https://www.tvbrowser.org/ and https://tvheadend.org/

Introduction

I am using the Tvheadend program (https://tvheadend.org/) on a Raspberry Pi with a USB TV receiver. I also use the TV-Browser program (https://www.tvbrowser.org/) on my Windows PC.

To program recordings, I use the Aufnahmesteuerung-Plug-in available in the TV browser program. Here you can define an external program for recording control. This external program takes over the communication with a TV tuner - in my case with Tvheadend.

For this purpose I developed the program AufnahmesteuerungTvHeadend. In this guide I describe the use and configuration of the program.

Function description

The program AufnahmesteuerungTvHeadend receives the necessary parameters via the command line and sends them to the TV headend server via network communication. The Rest-Api-Interface provided by Tvheadend is used. AufnahmesteuerungTvHeadend provides the two main functions of creating a recording and deleting a recording. In addition, the program provides functions for exporting a list of station names and a list of all programmed programs. In the documentation I describe the program with all parameters as well as all technical details for those interested in technology.


Release: 30.06.2025, Build 2.0.5 - Bug fixes

