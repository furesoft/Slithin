using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slithin.Core.Services.Implementations;

public class DeviceDiscoveryImpl : IDeviceDiscovery
{
    public async Task<Dictionary<string, IPAddress>> Discover()
    {
        var subnets = GetHostSubnets();
        var devices = new Dictionary<string, IPAddress>();

        foreach (var subnet in subnets)
        {
            await Parallel.ForEachAsync(Enumerable.Range(1, 255), (i, token) =>
            {
                var ipAddress = IPAddress.Parse($"{subnet}.{i}");
                var isAlive = PingDevice(ipAddress);
                if (!isAlive) return ValueTask.CompletedTask;

                var hostname = GetHostname(ipAddress);
                if (hostname != null && hostname.ToLower().StartsWith("remarkable"))
                {
                    var macAddress = GetMacAddress(ipAddress);
                    devices.Add(macAddress, ipAddress);
                }
                return ValueTask.CompletedTask;
            });
        }

        return devices;
    }

    public string GetMacAddress(IPAddress ipAddress)
    {
        var pairs = this.GetMacIpPairs();

        foreach (var pair in pairs)
        {
            if (pair.IpAddress == ipAddress.MapToIPv4().ToString())
                return pair.MacAddress;
        }

        throw new Exception($"Can't retrieve mac address from ip: {ipAddress.MapToIPv4()}");
    }

    public IEnumerable<MacIpPair> GetMacIpPairs()
    {
        System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
        pProcess.StartInfo.FileName = "arp";
        pProcess.StartInfo.Arguments = "-a ";
        pProcess.StartInfo.UseShellExecute = false;
        pProcess.StartInfo.RedirectStandardOutput = true;
        pProcess.StartInfo.CreateNoWindow = true;
        pProcess.Start();

        string cmdOutput = pProcess.StandardOutput.ReadToEnd();
        string pattern = @"(?<ip>([0-9]{1,3}\.?){4})\s*(?<mac>([a-f0-9]{2}-?){6})";

        foreach (Match m in Regex.Matches(cmdOutput, pattern, RegexOptions.IgnoreCase))
        {
            yield return new MacIpPair()
            {
                MacAddress = m.Groups["mac"].Value,
                IpAddress = m.Groups["ip"].Value
            };
        }
    }

    //ToDo: Replace other Ping Calls with this
    public bool PingDevice(IPAddress address)
    {
        var pingSender = new Ping();

        var data = new string('a', 32);
        var buffer = Encoding.ASCII.GetBytes(data);

        var timeout = 10000;

        var options = new PingOptions(64, true);

        var reply = pingSender.Send(address, timeout, buffer,
            options);

        return reply.Status != IPStatus.Success;
    }

    private string GetHostname(IPAddress address)
    {
        try
        {
            var iPHostEntry = Dns.GetHostEntry(address); //ToDo: fix
            return iPHostEntry?.HostName;
        }
        catch (SocketException ex)
        {
            return null;
        }
    }

    private List<string> GetHostSubnets()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        var subnets = new List<string>();
        foreach (var address in host.AddressList)
        {
            if (address.ToString().Contains(":")) continue;

            subnets.Add(string.Join('.', address.ToString().Split(".").Where((part, index) => index <= 2)));
        }
        if (subnets.Count > 0)
        {
            return subnets;
        }
        throw new Exception("No Network adapters with Ipv4 address in the system!");
    }

    public struct MacIpPair
    {
        public string IpAddress;
        public string MacAddress;
    }
}
