﻿namespace a.Models;

public class Request
{
    public string? action { get; set; }
    public int? cmdtype { get; set; }
    public string? token { get; set; }
    public int? subtype { get; set; }
    public Message? message { get; set; }
    public int? sequence { get; set; }
}
