using System;
using System.Collections.Generic;

namespace StrikeDetection.Models;

public partial class Status
{
    public int Id { get; set; }

    public int? Adcpktssent { get; set; }

    public int? Adctrigoff { get; set; }

    public int? Pgagain { get; set; }

    public int? Adcbase { get; set; }

    public int? Sysuptime { get; set; }

    public int? Netuptime { get; set; }

    public int? Gpsuptime { get; set; }

    public string Majorversion { get; set; }

    public int? Minorversion { get; set; }

    public int? Noise { get; set; }

    public int? Auxstatus { get; set; }

    public string Address { get; set; }

    public string Avgadcnoise { get; set; }

    public int? Batchid { get; set; }

    public string Clocktrim { get; set; }

    public int? Detector { get; set; }

    public string Packetnumber { get; set; }

    public string Packetssent { get; set; }

    public string Received { get; set; }

    public string Packettype { get; set; }

    public string Triggernoise { get; set; }

    public string Triggeroffset { get; set; }

    public DateTime? Stamp { get; set; }

    public string Lon { get; set; }

    public string Lat { get; set; }

    public int? Abovesealevel { get; set; }

    public long? Epoch { get; set; }

    public int? Temppress { get; set; }

    public int? Pressure { get; set; }

    public int? Temp { get; set; }

    public int? Udpsent { get; set; }

    public int? Clktrim { get; set; }

    public int? Satellites { get; set; }

    public string Geohash { get; set; }

    public int? Press { get; set; }

    public int? Peaklevel { get; set; }

    public int? Trigcount { get; set; }

    public int? Jabbering { get; set; }

    public int? Splatrev { get; set; }

    public int? Sensetype { get; set; }

    public int? Adcudpover { get; set; }

    public int? Udpcount { get; set; }

    public int? Bconf { get; set; }

    public int? Gpslocked { get; set; }

    public int? Epochsecs { get; set; }
}
