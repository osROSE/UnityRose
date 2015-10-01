UnityRose
===============================

#### Unity version of ROSE Online&#8482;


Contributors
-----------------------

UnityRose is only possible thanks to the fantastic
work of the following contributors:

<table><tbody>
<tr><th align="left">Wadii Bellamine</th><td><a href="https://github.com/liquidwad">GitHub/liquidwad</a></td><td><a href="http://twitter.com/skiliffon">Twitter/@skiliffon</a></td></tr>
<tr><th align="left">Brett Lawson</th><td><a href="https://github.com/brett19/">GitHub/brett19</a></td><td><a href="http://twitter.com/brett19x">Twitter/@brett19x</a></td></tr>
<tr><th align="left">Josh Ballard</th><td><a href="https://github.com/qix-/">GitHub/qix-</a></td><td><a href="http://twitter.com/IAmQix">Twitter/@IAmQix</a></td></tr>
</tbody></table>


The Official Client
-----------------------

Looking for the official ROSE Online game client?  Check out
[http://roseonlinegame.com/](http://roseonlinegame.com/) or visit 
[http://warpportal.com/](http://warpportal.com/) to get started!


Building
-----------------------
Building the project requires a little bit of setup, since we cannot include
all of the files for various reasons.

### 3ddata
You'll need to unpack the ROSE client's VFS files and place `3DDATA`'s content
into `Assets/3ddata`.

#### MipMap data
Mipmap data is currently bugged in the original DDS files. You'll need
ImageMagick installed and (preferably) a unix command line (whether it be on
linux, mac, or cygwin/msys on windows).

Run the following the commands:

```shell
$ cd Assets/3ddata
$ find . -type f -name "*.DDS" | xargs -L1 -I{} mogrify -define dds:mipmaps=0 "{}"
```

Be sure not to interrupt this command, and to only run it inside of the `3ddata`
folder! It can be hazardous otherwise!


Licence &amp; copyright
-----------------------

Copyright &copy; 2015 UnityRose contributors (listed above).

UnityRose source code is licensed under a Apache 2.0
License.  See the included 
[LICENSE](../blob/master/LICENSE) file for more details.
