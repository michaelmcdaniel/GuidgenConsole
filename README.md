<div style="word-wrap:pre">
GuidgenConsole
==============

Guidgen Console app generates, replaces, finds, and reformats GUIDs from the command line.

Guidgen console is a command line application that generates X number of guids in the format you want - pasting them to the clipboard - if you wish. But that's not all! It also allows pipe usage to find and replace GUIDs or just run in the background as a reformatter. Oh, the number of times I just needed to convert from one format to another... Try going from code to SQL to LDAP and you'll understand, but if you do understand, then this app is for you.

Features
 Guidgen works on the command line. No clunky windows app needed.
 Guidgen is a portable windows app (as long as you have the .NET framework - and who doesn't?)
 Can replace that antiquated Visual Studio Create GUID tool.
 Generate GUID(s) in any number of formats.
 Find GUID(s)
 Reformat GUID(s)
 Replace GUID(s) with new guids.

Guidgen can generate:
 Zero Guids
   00000000-0000-0000-0000-000000000000
 Sequential Guids
   c2d0e2e8-2b60-11e1-b1ea-0024e8359915
   c2d0e2e9-2b60-11e1-b1ea-0024e8359915
   c2d0e2ea-2b60-11e1-b1ea-0024e8359915
 Plain Vanilla Guids
   cf120f27-074c-43c0-80cb-57c1e3493c54
   f9018966-a96c-482c-b608-8695a95fdb0b
   a1065215-f596-445a-9e28-1aaba3a3e2be
  
Available Formats (See below for examples)
 N: 32 digits
 D: 32 digits with hyphens
 P: 32 digits enclosed in parentheses
 B: 32 digits enclosed in Brackets
 CP: c/c++ format
 GUID: c/c++ format with declaration
 DEFINE_GUID: c/c++ format with declaration
 OLECREATE: c++/COM format with declaration
 H: Hex byte array
 HC#: Hex byte array in c/c++/c# format
 HVB: Hex byte array in VB.NET format
 HLDAP: Hex byte array in LDAP format
 BASE64: Base64 of byte array - single instance
 BASE64C: Base64 of byte array - Combined bytes from all GUIDs
 
Usage
c:\> guidgen.exe /?

usage: GuidGen.exe [N|D|P|B|C|CP|H|HC#|HVB|HLDAP|BASE64|BASE64C] [/G|/S|/Z] [/nocopy] [/n (number)] [/u]

 Output Formats:
  N: 32 digits
  87654321dcbafe1054326789abcdef01
  D: 32 digits separated by hyphens (DEFAULT)
	00000000-0000-0000-0000-000000000000
  P: 32 digits separated by hyphens, enclosed in parentheses
	{00000000-0000-0000-0000-000000000000}
  B: 32 digits separated by hyphens, enclosed in brackets
	[00000000-0000-0000-0000-000000000000]
  C: c format
	0x00000000,0x0000,0x0000,0x0000,0x00,0x00,0x00,0x00,0x00,0x00
  CP: c format, enclosed in parentheses
	{0x00000000,0x0000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00}}
  GUID: c format, enclosed in parentheses
	static const GUID <> = [CP FORMAT];
  DEFINE_GUID: c format, enclosed in parentheses
	DEFINE_GUID(<>, [C FORMAT])
  OLECREATE: c format, enclosed in parentheses
	IMPLEMENT_OLECREATE(<>, <>, [C FORMAT])
  H: HEX byte array
	0123456789abcdef0123456789abcdef
  HC#: CSharp Hex byte array
	0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef,0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef
  HVB: VB Hex byte array
	&H01,&H23,&H45,&H67,&H89,&Hab,&Hcd,&Hef,&H01,&H23,&H45,&H67,&H89,&Hab,&Hcd,&Hef
  HLDAP: Hex byte array in ldap query form
	\\01\\23\\45\\67\\89\\ab\\cd\\ef\\01\\23\\45\\67\\89\\ab\\cd\\ef
  BASE64:
	AAAAAAAAAAAAAAAAAAAAAA==
  BASE64C: combine bytes to single base64 string
	AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=

 Type of GUID to create
  G: New Guid (default)
  Z: Zero Guid
  S: Sequential Guid

 Additional Arguments
  /u: returns format in uppercase (unless base64)
  /count (number): will generate the given number of guids
  /n (number): same as /count
  /Find (format): Finds guids in format (no copy)
  /l: shows line number for found guids
  /copy: forces copy to clipboard
  /nocopy: does not copy to clipboard
  /Replace: replaces guid with (/Z|/S|/G) or same guid to specified output format (nocopy) (no-BASE64C)
  /Replace [format]: replaces specified format with same guid or new guid if (/Z|/S|/G) is specified to specified output format (nocopy)
  /ReplaceByLine: like replace, but does everything per input line. (see above)
  /ReplaceByLine [format]: like replace, but does everything per input line. (see above)
  /guid (GUID): uses specified (GUID) as input for find and replace.
  /clipboard: uses clipboard for find and replace

 Notes:
   if find or replace is used and data is not piped in (ex: more find.txt | guidgen /find) then enter guids and then type "quit" to find/replace and end.
	

How or where do I install it?
 Install it where ever you want. I usually drop it into c:\windows so I can run it from anywhere.

How do I add it to Visual Studio Tools?
 Goto: Tools > External Tools
 Set the title, location where you saved it and any command line arguments.
 Click the "Use Output Window"
 Move it to the top.
 Click ok.

 Also add a quick key Ctrl-~
  Goto: Tools > Options > Environment > Keyboard 
  Select "Tools.ExternalCommand1" (if you moved it to the top...)
  Press the key combination. 
  Click ok. 
</div>