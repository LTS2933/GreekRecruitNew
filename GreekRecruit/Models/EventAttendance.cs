﻿using System.ComponentModel.DataAnnotations;
namespace GreekRecruit.Models;
public class EventAttendance
{
    [Key]
    public int attendance_id { get; set; }

    public int event_id { get; set; }

    public string pnm_fname { get; set; }

    public string pnm_lname { get; set; }

    public string pnm_schoolyear { get; set; }

    public DateTime checked_in_at { get; set; } = DateTime.Now;
}
