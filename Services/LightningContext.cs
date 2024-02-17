using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StrikeDetection.Models;

namespace StrikeDetection.Services;

public interface IDbService
{
    Task<int> AddStatus(Status status);
    Task<int> AddSample(Sample status);
    Task<Status[]> GetLastXStatusAsync(int count, int detector = 0);
    Task<Sample[]> GetLastXSampleAsync(int count, int detector = 0);
    Task<DetectorInfo[]> GetAllDetectorsAsync();
}

public partial class LightningContext : DbContext, IDbService
{
    private int MAX_ROWS = 1000;
    #region IDBService
    public async Task<int> AddSample(Sample sample)
    {
        return await StoreSampleAsync(sample);
    }

    public Task<Sample[]> GetLastXSampleAsync(int count, int detector = 0)
    {
        return GetSamples(detector).Take(Math.Min(count, MAX_ROWS)).ToArrayAsync();
    }

    public IOrderedQueryable<Sample> GetSamples(int detector = 0)
    {
        if (detector != 0) return Samples.OrderByDescending(x => x.Id);
        return Samples.Where(x => x.Detector == detector).OrderByDescending(x => x.Id);
    }


    public async Task<int> AddStatus(Status status)
    {
        return await StoreStatusAsync(status);
    }

    public async Task<Status[]> GetLastXStatusAsync(int count, int detector = 0)
    {
        var set = Statuses.OrderByDescending(x => x.Id).Take(Math.Min(count, MAX_ROWS));
        if (detector != 0)
            set = set.Where(x => x.Detector == detector);
        return await set.ToArrayAsync();
    }
    #endregion

    internal async Task<int> StoreStatusAsync(Status status)
    {
        Statuses.Add(status);
        return await SaveChangesAsync();
    }
    private async Task<int> StoreSampleAsync(Sample sample)
    {
        Samples.Add(sample);
        return await SaveChangesAsync();
    }


    internal async Task<int> StoreDetectorAsync(DetectorInfo detectorInfo)
    {
        DetectorInfos.Add(detectorInfo);
        return await SaveChangesAsync();
    }
    public async Task<DetectorInfo[]> GetAllDetectorsAsync()
    {
        return await DetectorInfos.Take(MAX_ROWS).ToArrayAsync();
    }

    #region Model

    public LightningContext(DbContextOptions<LightningContext> options)
        : base(options)
    {
        this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }


    public virtual DbSet<DetectorInfo> DetectorInfos { get; set; }

    public virtual DbSet<Sample> Samples { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetectorInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("detector_info_pkey");

            entity.ToTable("detector_info");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("display_name");
        });

        modelBuilder.Entity<Sample>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sample_pkey");

            entity.ToTable("sample");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AbsoluteTime).HasColumnName("absolute_time");
            entity.Property(e => e.Adcseq).HasColumnName("adcseq");
            entity.Property(e => e.Batchid).HasColumnName("batchid");
            entity.Property(e => e.Data)
                .IsRequired()
                .HasColumnName("data");
            entity.Property(e => e.Detector).HasColumnName("detector");
            entity.Property(e => e.Dmatime).HasColumnName("dmatime");
            entity.Property(e => e.PeakSample).HasColumnName("peak_sample");
            entity.Property(e => e.Rtsecs).HasColumnName("rtsecs");
            entity.Property(e => e.Smoothed).HasColumnName("smoothed");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("timestamp");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("status");

            entity.Property(e => e.Abovesealevel).HasColumnName("abovesealevel");
            entity.Property(e => e.Adcbase).HasColumnName("adcbase");
            entity.Property(e => e.Adcpktssent).HasColumnName("adcpktssent");
            entity.Property(e => e.Adctrigoff).HasColumnName("adctrigoff");
            entity.Property(e => e.Adcudpover).HasColumnName("adcudpover");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Auxstatus).HasColumnName("auxstatus");
            entity.Property(e => e.Avgadcnoise).HasColumnName("avgadcnoise");
            entity.Property(e => e.Batchid).HasColumnName("batchid");
            entity.Property(e => e.Bconf).HasColumnName("bconf");
            entity.Property(e => e.Clktrim).HasColumnName("clktrim");
            entity.Property(e => e.Clocktrim).HasColumnName("clocktrim");
            entity.Property(e => e.Detector).HasColumnName("detectorid");
            entity.Property(e => e.Epoch).HasColumnName("epoch");
            entity.Property(e => e.Epochsecs).HasColumnName("epochsecs");
            entity.Property(e => e.Geohash).HasColumnName("geohash");
            entity.Property(e => e.Gpslocked).HasColumnName("gpslocked");
            entity.Property(e => e.Gpsuptime).HasColumnName("gpsuptime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Jabbering).HasColumnName("jabbering");
            entity.Property(e => e.Lat).HasColumnName("lat");
            entity.Property(e => e.Lon).HasColumnName("lon");
            entity.Property(e => e.Majorversion).HasColumnName("majorversion");
            entity.Property(e => e.Minorversion).HasColumnName("minorversion");
            entity.Property(e => e.Netuptime).HasColumnName("netuptime");
            entity.Property(e => e.Noise).HasColumnName("noise");
            entity.Property(e => e.Packetnumber).HasColumnName("packetnumber");
            entity.Property(e => e.Packetssent).HasColumnName("packetssent");
            entity.Property(e => e.Packettype).HasColumnName("packettype");
            entity.Property(e => e.Peaklevel).HasColumnName("peaklevel");
            entity.Property(e => e.Pgagain).HasColumnName("pgagain");
            entity.Property(e => e.Press).HasColumnName("press");
            entity.Property(e => e.Pressure).HasColumnName("pressure");
            entity.Property(e => e.Received).HasColumnName("received");
            entity.Property(e => e.Satellites).HasColumnName("satellites");
            entity.Property(e => e.Sensetype).HasColumnName("sensetype");
            entity.Property(e => e.Splatrev).HasColumnName("splatrev");
            entity.Property(e => e.Stamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("stamp");
            entity.Property(e => e.Sysuptime).HasColumnName("sysuptime");
            entity.Property(e => e.Temp).HasColumnName("temp");
            entity.Property(e => e.Temppress).HasColumnName("temppress");
            entity.Property(e => e.Trigcount).HasColumnName("trigcount");
            entity.Property(e => e.Triggernoise).HasColumnName("triggernoise");
            entity.Property(e => e.Triggeroffset).HasColumnName("triggeroffset");
            entity.Property(e => e.Udpcount).HasColumnName("udpcount");
            entity.Property(e => e.Udpsent).HasColumnName("udpsent");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    #endregion

}
