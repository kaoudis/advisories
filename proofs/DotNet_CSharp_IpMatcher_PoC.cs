﻿/* Author: Kelly Kaoudis
 * License: GPLv3
 * 
 * Requires:
 * `dotnet add package IpMatcher --version 1.0.4.1`
 * 
 * To run: 
 * `dotnet run`
 */

using System;
using IpMatcher;

namespace dotnet
{
    class PoC
    {
        private static void checkExists(Matcher matcher, string ip, string mask)
        {
            if (matcher.Exists(ip, mask))
            {
                Console.WriteLine("matches on " + ip + " / " + mask);
            } 
            else
            {
                Console.WriteLine("DOES NOT match on " + ip + " / " + mask);
            }
        }
        
        private static void checkMatchExists(Matcher matcher, string ip)
        { 
            if (matcher.MatchExists(ip))
            {
                Console.WriteLine("matches on " + ip);
            } 
            else
            {
                Console.WriteLine("DOES NOT match on " + ip);
            }
        
        }

        private static void dumpMatcher(Matcher matcher)
        {
            Console.WriteLine("\nWhat is actually in the matcher now (if nothing follows on the next line, nothing)?");
            foreach (string addr in matcher.All())
            { 
                Console.WriteLine("address from matcher: " + addr);
            }
            Console.WriteLine("");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Constructing a new IpMatcher#Matcher...");
            Matcher matcher = new Matcher();
            // nothing in the matcher yet
            dumpMatcher(matcher);

            Console.WriteLine("adding 192.31.196.0 / 0.0.0.0 (mask)");
            matcher.Add("192.31.196.0", "0.0.0.0");

            // contains 0.0.0.0 / 0.0.0.0 (incorrect)
            dumpMatcher(matcher);

            checkExists(matcher, "192.31.196.2", "0.0.0.0");
            checkExists(matcher, "192.31.196.1", "0.0.0.0");
            checkExists(matcher, "192.31.196.0", "0.0.0.0"); // should match but does not
            checkExists(matcher, "0.0.0.0", "255.0.0.0"); //should not match
            checkExists(matcher, "0.0.0.0", "0.0.0.0"); 

            checkMatchExists(matcher, "0.0.0.0"); 
            checkMatchExists(matcher, "192.31.196.0");
            checkMatchExists(matcher, "192.31.196.1");
            //checkMatchExists(matcher, "0192.031.0196.0"); throws parse exception and not sure why
            checkMatchExists(matcher, "0300.037.0304.0"); // octal for 192.31.196.0
            checkMatchExists(matcher, "0300.037.0304.01");
            checkMatchExists(matcher, "0300.036.0304.0"); // should not match but does
            checkMatchExists(matcher, "0100.0100.0100.0100"); // should not match but does

        //    checkMatchExists(matcher, "aaaaaaaaaa"); thankfully results in exception 

            // results in invalid argument exception
           // if (matcher.MatchExists("0192.031.0196.02"))
           // {
           //     Console.WriteLine("gross! matches 0192.031.0196.02");
           // }
           
            Console.WriteLine("adding 192.168.0.0 / 255.0.0.0 (mask)");
            matcher.Add("192.168.0.0", "255.0.0.0");

            checkExists(matcher, "192.167.0.1", "255.0.0.0");
            checkExists(matcher, "192.168.0.0", "255.0.0.0");
            checkExists(matcher, "192.168.1.1", "255.0.0.0");
            checkMatchExists(matcher, "172.13.2.15");
            checkMatchExists(matcher, "010.1.1.1");
            checkMatchExists(matcher, "4.4.4.4");

            Console.WriteLine("adding 0300.055.0250.0 / 1.1.0.0 (mask)");
            matcher.Add("0300.055.0250.0", "1.1.0.0");

            checkExists(matcher, "192.45.168.0", "1.1.0.0");
            checkExists(matcher, "0300.055.0250.0", "0.0.0.0");
            checkExists(matcher, "0300.055.0250.0300", "1.1.0.0");
            checkExists(matcher, "0288.055.0250.0", "1.1.0.0");

            checkMatchExists(matcher, "2130706433");
            checkMatchExists(matcher, "017700000001");
            checkMatchExists(matcher, "3232235521");
            checkMatchExists(matcher, "3232235777");
            checkMatchExists(matcher, "0x7f.0x00.0x00.0x01");
            checkMatchExists(matcher, "0xc0.0xa8.0x00.0x14");

            Console.WriteLine("adding 0300.055.0250.0 / 0377.0.0.0 (mask)");
            matcher.Add("0300.055.0250.0", "0377.0.0.0");

            Console.WriteLine("adding 0250.0300.010.010 / 0.0.0.0 (mask)");
            matcher.Add("0250.0300.010.010", "0.0.0.0");

            Console.WriteLine("adding 0250.0300.010.010 / 010.010.010.0 (mask)");
            matcher.Add("0250.0300.010.010", "010.010.010.0");

            // anything ending in 8 or 9 doesn't work
            Console.WriteLine("adding 0172.057.0.0 / 0.0.0.0 (mask)");
            matcher.Add("0172.057.0.0", "0.0.0.0");

            Console.WriteLine("adding 0172.057.0.0 / 055.055.013.0 (mask)");
            matcher.Add("0172.057.0.0", "055.055.013.0");

         //   matcher.Add("08.09.0.0", "01.01.01.0"); fails as it should

            Console.WriteLine("adding 010.010.0172.0 / 0.0.0.0 (mask)");
            matcher.Add("010.010.0172.0", "0.0.0.0");

            Console.WriteLine("adding 010.010.0172.0 / 01.01.01.01 (mask)");
            matcher.Add("010.010.0172.0", "01.01.01.01");
            
            Console.WriteLine("adding 010.010.0172.0 / 010.010.0172.010 (mask)");
            matcher.Add("010.010.0172.0", "010.010.0172.010");

            Console.WriteLine("adding 010.010.0172.0 / 010.010.0.010 (mask)");
            matcher.Add("010.010.0172.0", "010.010.0.010");

            Console.WriteLine("adding 010.010.0172.0 / 010.010.0.010 (mask)");
            matcher.Add("010.010.0172.0", "010.010.0255.010");

            Console.WriteLine("adding 0xaa.0xaa.0xaa.0xaa / 0xaa.0xfe.0xfe.0xfe (mask)");
            matcher.Add("0xaa.0xaa.0xaa.0xaa", "0xfe.0xfe.0xfe.0xfe");

          //  fails with exception as it should as 0xfff is tooooo biggggg
          //  matcher.Add("0xfff.0xfff.0xfff.0x0", "0x0.0x0.0x0.0x0");

            Console.WriteLine("adding 0xf0.0x0.0x0.0x0 / 0xff.0x0.0x0.0x0 (mask)");
            matcher.Add("0xf0.0x0.0x0.0x0", "0xff.0x0.0x0.0x0");
            
            // now contains the following:
            // 0.0.0.0/0.0.0.0
            // 192.0.0.0/255.0.0.0
            // 0.1.0.0/1.1.0.0
            // 192.0.0.0/0377.0.0.0
            // 8.0.8.0/010.010.010.0
            // 40.45.0.0/055.055.013.0
            // 8.8.122.0/010.010.0172.010
            // 8.8.0.0/010.010.0.010
            // 8.8.40.0/010.010.0255.010
            // 170.170.170.170/0xfe.0xfe.0xfe.0xfe
            // 240.0.0.0/0xff.0x0.0x0.0x0
            dumpMatcher(matcher);
        }
    }
}
