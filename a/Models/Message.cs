namespace a.Models;

public class Message
{
    public string? username { get; set; }   
    public string? password { get; set; }
    public int? atmo_trans { get; set; }
    public int? color_bar { get; set; }
    public int? color_show { get; set; }
    public double? dist { get; set; }
    public double? emiss { get; set; }
    public int? emiss_mode { get; set; }
    public int? flag { get; set; }
    public int? gear { get; set; }
    public int? humi { get; set; }
    public int? isot_flag { get; set; }
    public int? isot_high { get; set; }
    public string? isot_high_color { get; set; }
    public int? isot_low { get; set; }
    public string? isot_low_color { get; set; }
    public int? isot_type { get; set; }
    public int? mod_temp { get; set; }
    public int? opti_trans { get; set; }
    public int? ref_temp { get; set; }
    public string? show_desc { get; set; }
    public int? show_mode { get; set; }
    public int? show_mouse { get; set; }
    public int? show_string { get; set; }
    public string? query_start_time {get; set; }
    public string? query_end_time { get; set; }
    public int? query_type { get; set; }
    public int? query_interval { get; set; }
    public int? query_temp_type { get; set; }
    public int? report { get; set; }
    public int? page_index { get; set; }
    public int? page_count { get; set; }
}