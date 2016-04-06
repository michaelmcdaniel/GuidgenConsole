<h2>GuidgenConsole</h2>

<h3>Guidgen Console app generates, replaces, finds, and reformats GUIDs from the command line.</h3>
<p>
Guidgen console is a command line application that generates X number of guids in the format you want - pasting them to the clipboard - if you wish. But that's not all! It also allows pipe usage to find and replace GUIDs or just run in the background as a reformatter. Oh, the number of times I just needed to convert from one format to another... Try going from code to SQL to LDAP and you'll understand, but if you do understand, then this app is for you.
</p>

<h4>Features</h4>
<ul>
	<li>Guidgen works on the command line. No clunky windows app needed.</li>
 	<li>Guidgen is a portable windows app (as long as you have the .NET framework - and who doesn't?)</li>
 	<li>Can replace that antiquated Visual Studio Create GUID tool.</li>
 	<li>Generate GUID(s) in any number of formats.</li>
 	<li>Find GUID(s)</li>
 	<li>Reformat GUID(s)</li>
 	<li>Replace GUID(s) with new guids.</li>
</ul>

<h4>Guidgen can generate</h4>
<ul>
	<li>Zero Guids
   	<ul><li>00000000-0000-0000-0000-000000000000</li></ul>
  </li>
	<li>Sequential Guids
   <ul><li>c2d0e2e8-2b60-11e1-b1ea-0024e8359915</li>
   <li>c2d0e2e9-2b60-11e1-b1ea-0024e8359915</li>
   <li>c2d0e2ea-2b60-11e1-b1ea-0024e8359915</li></ul>
  </li>
	<li>Plain Vanilla Guids
   <ul><li>cf120f27-074c-43c0-80cb-57c1e3493c54</li>
   <li>f9018966-a96c-482c-b608-8695a95fdb0b</li>
   <li>a1065215-f596-445a-9e28-1aaba3a3e2be</li></ul>
  </li>
</ul>
 
<h4>Usage</h4>
<pre>
c:\> guidgen.exe /?

usage: GuidGen.exe [N|D|P|B|C|CP|H|HC#|HVB|HLDAP|BASE64|BASE64C] [/G|/S|/Z] [/nocopy] [/n (number)] [/u]<br/>
<h6>Output Formats:</h6>
  <b>N</b>: 32 digits
      <i>87654321dcbafe1054326789abcdef01</i>
  <b>D</b>: 32 digits separated by hyphens (DEFAULT)
      <i>00000000-0000-0000-0000-000000000000</i>
  <b>P</b>: 32 digits separated by hyphens, enclosed in parentheses
      <i>{00000000-0000-0000-0000-000000000000}</i>
  <b>B</b>: 32 digits separated by hyphens, enclosed in brackets
      <i>[00000000-0000-0000-0000-000000000000]</i>
  <b>C</b>: c format
      <i>0x00000000,0x0000,0x0000,0x0000,0x00,0x00,0x00,0x00,0x00,0x00</i>
  <b>CP</b>: c format, enclosed in parentheses
      <i>{0x00000000,0x0000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00}}</i>
  <b>GUID</b>: c format, enclosed in parentheses
      <i>static const GUID <> = [CP FORMAT];</i>
  <b>DEFINE_GUID</b>: c format, enclosed in parentheses
      <i>DEFINE_GUID(<>, [C FORMAT])</i>
  <b>OLECREATE</b>: c format, enclosed in parentheses
      <i>IMPLEMENT_OLECREATE(<>, <>, [C FORMAT])</i>
  <b>H</b>: HEX byte array
      <i>0123456789abcdef0123456789abcdef</i>
  <b>HC#</b>: CSharp Hex byte array
      <i>0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef,0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef</i>
  <b>HVB</b>: VB Hex byte array
      <i>&H01,&H23,&H45,&H67,&H89,&Hab,&Hcd,&Hef,&H01,&H23,&H45,&H67,&H89,&Hab,&Hcd,&Hef</i>
  <b>HLDAP</b>: Hex byte array in ldap query form
      <i>\\01\\23\\45\\67\\89\\ab\\cd\\ef\\01\\23\\45\\67\\89\\ab\\cd\\ef</i>
  <b>ORACLE</b>: ORACLE Hex byte array
      <i>8967452301cdab01ef23456789abcdef</i>
  <b>ORACLE_HEXTORAW</b>: ORACLE Hex byte array with declaration
      <i>HEXTORAW('8967452301cdab01ef23456789abcdef')</i>
  <b>BASE64</b>:
      <i>AAAAAAAAAAAAAAAAAAAAAA==</i>
  <b>BASE64C</b>: combine bytes to single base64 string
      <i>AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=</i>
<h6>Type of GUID to create</h6>
  <b>G</b>: New Guid (default)
  <b>Z</b>: Zero Guid
  <b>S</b>: Sequential Guid
<h6>Additional Arguments</h6>
  <b>/u</b>: returns format in uppercase (unless base64)
  <b>/count (number)</b>: will generate the given number of guids
  <b>/n (number)</b>: same as /count
  <b>/Find (format)</b>: Finds guids in format (no copy)
  <b>/l</b>: shows line number for found guids
  <b>/copy</b>: forces copy to clipboard
  <b>/nocopy</b>: does not copy to clipboard
  <b>/Replace</b>: replaces guid with (/Z|/S|/G) or same guid to specified output format (nocopy) (no-BASE64C)
  <b>/Replace [format]</b>: replaces specified format with same guid or new guid if (/Z|/S|/G) is specified to specified output format (nocopy)
  <b>/ReplaceByLine</b>: like replace, but does everything per input line. (see above)
  <b>/ReplaceByLine [format]</b>: like replace, but does everything per input line. (see above)
  <b>/guid (GUID)</b>: uses specified (GUID) as input for find and replace.
  <b>/clipboard</b>: uses clipboard for find and replace
<h6>Notes:</h6>
   if find or replace is used and data is not piped in (ex: more find.txt | guidgen /find) 
   then enter guids and then type "quit" to find/replace and end.
</pre>	

<h4>How or where do I install it?</h4>
 Install it where ever you want. I usually drop it into c:\windows so I can run it from anywhere.<br/><br/>
 Guidgen requires the .NET Framework 4.6.1 available at: <a href="http://go.microsoft.com/fwlink/?LinkId=671729">http://go.microsoft.com/fwlink/?LinkId=671729</a>

<h4>How do I add it to Visual Studio Tools?</h4>
 Goto: Tools > External Tools<br/>
 Set the title, location where you saved it and any command line arguments.<br/>
 Click the "Use Output Window"<br/>
 Move it to the top.<br/>
 Click ok.<br/>

 <h5>Also add a quick key Ctrl-~</h5>
  Goto: Tools > Options > Environment > Keyboard <br/>
  Select "Tools.ExternalCommand1" (if you moved it to the top...)<br/>
  Press the key combination. <br/>
  Click ok. <br/>
</pre>
