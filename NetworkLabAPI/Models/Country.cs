using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLabAPI.Models;
public record Country(
    Name Name,
    string[] Capital,
    double Population,
    Dictionary<string, Currency> Currencies,
    Dictionary<string, string> Languages,
    string Region,
    string Subregion,
    double? Area,
    string[] Timezones,
    Flags Flags);

public record Name(
    string Common,
    string Official,
    Dictionary<string, NativeName> NativeName);

public record NativeName(string Official, string Common);
public record Currency(string Name, string Symbol);
public record Flags(string Png, string Svg, string Alt);