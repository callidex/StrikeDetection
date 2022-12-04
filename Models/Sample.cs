using System;
using System.Collections.Generic;

namespace StrikeDetection.Models;

public partial class Sample
{
    public int Id { get; set; }

    public int Detector { get; set; }

    public int[] Data { get; set; }

    public int Dmatime { get; set; }

    public int Batchid { get; set; }

    public long Rtsecs { get; set; }

    public int Adcseq { get; set; }

    public int[] Smoothed { get; set; }

    public DateTime? Timestamp { get; set; }

    public int? PeakSample { get; set; }

    public long? AbsoluteTime { get; set; }
}
