<h2>GuidgenConsole</h2>

<h3>Guidgen Console app generates, replaces, finds, and reformats GUIDs from the command line.</h3>
<p>
Guidgen console (aka Guid Generator) is a command line application that generates X number of guids in the format you want - pasting them to the clipboard - if you wish. But that's not all! It also allows pipe usage to find and replace GUIDs or just run in the background as a reformatter. Oh, the number of times I just needed to convert from one format to another... Try going from code to SQL to LDAP and you'll understand, but if you do understand, then this app is for you.
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

usage: GuidGen.exe [N|D|P|B|C|CP|GUID|OLECREATE|DEFINE_GUID|H|HC#|HVB|HLDAP|<br/>ORACLE|ORACLE_HEXTORAW|IP|VERSION|INT32|INT64|BASE64|BASE64C|MD5-[FORMAT]]<br/>[/G|/S|/Z] [/nocopy] [/n (number)] [/u]<br/>
<h6>Output Formats:</h6>
   <b>N</b>: 32 digits
	<i>0102030405060708090a0b0c0d0e0f10</i>
   <b>D</b>: 32 digits separated by hyphens (DEFAULT)
	<i>01020304-0506-0708-090a-0b0c0d0e0f10</i>
   <b>P</b>: 32 digits separated by hyphens, enclosed in (curly) braces
	<i>{01020304-0506-0708-090a-0b0c0d0e0f10}</i>
   <b>B</b>: 32 digits separated by hyphens, enclosed in brackets
	<i>[01020304-0506-0708-090a-0b0c0d0e0f10]</i>
   <b>C</b>: c format
	<i>0x01020304,0x0506,0x0708,0x090a,0x0b,0x0c,0x0d,0x0e,0x0f,0x10</i>
   <b>CP</b>: c format, enclosed in (curly) braces
	<i>{0x01020304,0x0506,0x0708,0x090a,{0x0b,0x0c,0x0d,0x0e,0x0f,0x10}}</i>
   <b>GUID</b>: c format, enclosed in (curly) braces
	<i>static const GUID &lt;&lt;name&gt;&gt; = 0x01020304,0x0506,0x0708,0x090a,0x0b,0x0c,0x0d,0x0e,0x0f,0x10;</i>
   <b>DEFINE_GUID</b>: c format, enclosed in (curly) braces
	<i>DEFINE_GUID(&lt;&lt;name&gt;&gt;,0x01020304,0x0506,0x0708,0x090a,0x0b,0x0c,0x0d,0x0e,0x0f,0x10)</i>
   <b>OLECREATE</b>: c format, enclosed in (curly) braces
	<i>IMPLEMENT_OLECREATE(&lt;&lt;class>>,&lt;&lt;external_name&gt;&gt;,0x01020304,0x0506,0x0708,0x090a,0x0b,0x0c,0x0d,0x0e,0x0f,0x10)</i>
   <b>H</b>: HEX byte array
	<i>0403020106050807090a0b0c0d0e0f10</i>
   <b>HC#</b>: CSharp Hex byte array
	<i> 0x04,0x03,0x02,0x01,0x06,0x05,0x08,0x07,0x09,0x0a,0x0b,0x0c,0x0d,0x0e,0x0f,0x10</i>
   <b>HVB</b>: VB Hex byte array
	<i>&amp;H04,&amp;H03,&amp;H02,&amp;H01,&amp;H06,&amp;H05,&amp;H08,&amp;H07,&amp;H09,&amp;H0a,&amp;H0b,&amp;H0c,&amp;H0d,&amp;H0e,&amp;H0f,&amp;H10</i>
   <b>HLDAP</b>: Hex byte array in ldap query form
	<i>\\04\\03\\02\\01\\06\\05\\08\\07\\09\\0a\\0b\\0c\\0d\\0e\\0f\\10</i>
   <b>ORACLE</b>: ORACLE Hex byte array
	<i>04030201-0605-0807-090a-0b0c0d0e0f10</i>
   <b>ORACLE_HEXTORAW</b>: ORACLE Hex byte array with declaration
	<i>HEXTORAW('04030201-0605-0807-090a-0b0c0d0e0f10')</i>
   <b>IP</b>: IP Address format (IPv4/IPv6)
	<i>403:201:605:807:90a:b0c:d0e:f10</i>
   <b>Version</b>: Version format (Major.Minor.Build.Revision)
	<i>16909060.117966086.202050057.269422093</i>
   <b>Int32</b>: Int32 format (Int32, Int32, Int32, Int32)
	<i>16909060, 117966086, 202050057, 269422093</i>
   <b>Int64</b>: Int64 format (Int64, Int64)
	<i>506660481424032516, 1157159078456920585</i>
   <b>Int128</b>: Int128/BitInteger
	<i>21345817372864405881775281973894447876</i>
   <b>BASE64</b>:
	<i>BAMCAQYFCAcJCgsMDQ4PEA==</i>
   <b>BASE64C</b>: combine bytes to single base64 string
	<i>AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=</i>
   <b>MD5-[FORMAT]</b>: MD5 input string to Guid in the selected format (must use /find)
	

<h6>Type of GUID to create</h6>
  <b>G</b>: New Guid (default)
  <b>Z</b>: Zero Guid
  <b>S</b>: Sequential Guid
<h6>Additional Arguments</h6>
  <b>/u</b>: returns format in uppercase (unless base64)
  <b>/count (number)</b>: will generate the given number of guids
  <b>/n (number)</b>: same as /count
  <b>/Find [format]</b>: Finds guids in format
  <b>/l</b>: shows line number for found guids
  <b>/copy</b>: forces copy to clipboard
  <b>/nocopy</b>: does not copy to clipboard
  <b>/Replace</b>: replaces guid with (/Z|/S|/G) or same guid to specified output format
  <b>/Replace [format]</b>: replaces specified format with same guid or new guid if (/Z|/S|/G) is specified to specified output format
  <b>/ReplaceByLine</b>: like replace, but does everything per input line. (see above)
  <b>/ReplaceByLine [format]</b>: like replace, but does everything per input line. (see above)
  <b>/guid (GUID)</b>: uses specified (GUID) as input for find and replace.
  <b>/clipboard</b>: uses clipboard for find and replace
<h6>Notes:</h6>
   if find or replace is used and data is not piped in (ex: more find.txt | guidgen /find) 
   then enter guids and then type "quit" to find/replace and end.
</pre>	
<h4>Where can I get it?</h4>
Latest binaries in the binaries folder of the project or use powershell and <a href="https://chocolatey.org">chocolatey</a>!  <i>v2.0.0.3+ - Chocolately install will download the binary from github and try to install the highest matching .NET version.  If no matching versions, you get the latest version of 4.8.</i>
<code><pre>
PS C:\> choco install guidgen-console
</pre></code>

<h4>What dependencies does it have?</h4>
The latest version is built against the .NET Framework version 4.7.<br/>
available at: <a href="http://go.microsoft.com/fwlink/?LinkId=671729">http://go.microsoft.com/fwlink/?LinkId=671729</a><br/>
There are binaries available for .NET frameworks: 2.0,3.5,4.0,4.5,4.6,4.7,4.8

<h4>Where do I install it?</h4>
Install it where ever you want!<br/>
I personally use chocolatey which installs a shim in <span title="C:\ProgramData\chocolatey\bin"><i>%ALLUSERSPROFILE%\chocolatey\bin </i></span> that points to <span title="C:\ProgramData\chocolatey\bin"><i>%ALLUSERSPROFILE%\chocolatey\lib\guidgen-console\guidgen.exe </i></span>.  The shim is already in the path, so you can run it from anywhere!

<h4>Can I change the defaults?</h4>
<b>Yes!</b> Add (or change) <i>guidgen.exe.config</i><br/>
If you used chocolatey to install, the file should be in <span title="C:\ProgramData\chocolatey\lib\guidgen-console\"><i>%ALLUSERSPROFILE%\chocolatey\lib\guidgen-console\ </i></span>
<code><pre>&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
&lt;configuration&gt;
	&lt;appSettings&gt;
		&lt;add key=&quot;default:output:format&quot; value=&quot;D&quot; /&gt;
		&lt;add key=&quot;default:guid:type&quot; value=&quot;G&quot; /&gt;
		&lt;add key=&quot;default:example&quot; value=&quot;01020304-0506-0708-090a-0b0c0d0e0f10&quot; /&gt;
	&lt;/appSettings&gt;
&lt;/configuration&gt;</pre></code>


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
  Click ok.

<h4>Example Usage</h4>
Generate a guid:
<pre><code>c:\> guidgen D
3dbacc0e-940a-4386-bce9-adc285f45ffe</code></pre>
Generate a upper-cased guid:
<pre><code>c:\> guidgen D /u
2EE89794-7759-4425-80DC-126F73264B30</code></pre>
Generate 3 sequential guids:
<pre><code>c:\> guidgen D /S /n 3
21638464-8ef4-11e1-acb0-0024e8359915
21638465-8ef4-11e1-acb0-0024e8359915
21638466-8ef4-11e1-acb0-0024e8359915</code></pre>
Replace guid with another format:
<pre><code>c:\> guidgen HC# /replace /guid 21638464-8ef4-11e1-acb0-0024e8359915
0x64,0x84,0x63,0x21,0xf4,0x8e,0xe1,0x11,0xac,0xb0,0x00,0x24,0xe8,0x35,0x99,0x15</code></pre>
<pre><code>c:\> guidgen D /replace H /guid "64 84 63 21 f4 8e e1 11 ac b0 00 24 e8 35 99 15"
21638464-8ef4-11e1-acb0-0024e8359915</code></pre>
Replace guid with another guid in same format: <i>(replace format may be required.)</i>
<pre><code>c:\> guidgen /G /replace D /guid 21638464-8ef4-11e1-acb0-0024e8359915
aad71ce1-31d9-409b-ba83-398c8a62cdec</code></pre>
Find Guids (with line numbers):
<pre><code>c:\> guidgen N /n 3 > guids.txt
c:\> more guids.txt | guidgen D /find /l
Ln: 0 Col: 0    aa4a2c8a-37ac-432d-afc2-40f62b1d01a2    aa4a2c8a37ac432dafc240f62b1d01a2
Ln: 1 Col: 0    70e5d73a-6b06-4b3e-a473-cdbbedf85f61    70e5d73a6b064b3ea473cdbbedf85f61
Ln: 2 Col: 0    4bae3611-fdc7-4af3-b47a-b36e5fc9d45a    4bae3611fdc74af3b47ab36e5fc9d45a</code></pre>
Reformat guids:
<pre><code>c:\> guidgen P /u /replacebyline
Type "quit" to quit
&gt; c272825f-79ab-4fcc-b265-e07c488ed8ae
{C272825F-79AB-4FCC-B265-E07C488ED8AE}
&gt; quit</code></pre>
Any string to guid:
<pre><code>c:\> guidgen MD5-D /find MD5-D /replace /guid "MD5 String 2 Guid?"
6c0a3a7d-05bc-429d-f6ca-502b1fbb23df</code></pre>
<pre><code>c:\> guidgen MD5-D /replacebyline MD5-D
Type "quit" to quit
&gt; Why do this?
488bcdc4-7ed5-4f10-c644-4d655cf8663e
&gt; I Don't Know
4bd31432-d910-a870-b617-e1a1666c67a9
&gt; I Don't Know
4bd31432-d910-a870-b617-e1a1666c67a9
&gt; Because we can!
b8ecc945-c302-dd2d-7921-9081211480b1
&gt; quit</code></pre>

