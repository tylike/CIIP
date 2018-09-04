# CIIP 
商信互联企业信息应用平台

网站:http://www.uims.top<BR>
QQ群:336090194<BR>
QQ:460325<BR>
欢迎加入讨论<BR>
XAF技术博客:http://www.cnblogs.com/foreachlife/<BR>

开发环境:visual studio 2017 + devexpress application framework (XAF)18.1 + SqlServer2016<BR>

简介:<BR>
CIIP是用于快速构建企业级信息系统的工具,基于XAF开发.<BR>
CIIP.Designer : 用于快速的,以信息表单方式维护元数据.与传统的编码方式相比,在C#中支持类似于多重继承的功能.<BR>
CIIP.Client : 使用CIIP.Designer的输出项目，展示给最终客户。<BR>
将会区分为WinForm版本，Web版本，Mobile版本。<BR>
CIIP是高度模块化的设计的，用户根据需要选择对应的模块。
CIIP是高度可自定义的，用户可以在运行时配置很多内容。


XAF是一个不错的面向开发人员的商业框架,由DevExpress公司开发.如果需要进行开发工作请购买正版,网址:http://www.devexpress.com <BR>
一,CIIP的目标是什么?

更加简单,快速的建立信息类管理系统.让实施人员可能承担多数工作,降低开发人员的劳动强度.

二,CIIP改动了哪些?

2016年到本次更新之前,CIIP的开发范围很大,本次策略更改为小版本快速更新.但去掉了很多以前的功能.以前的功能虽然存在,但有很多BUG.后面将小功能的快速增加.

今天CIIP只有一些基本功能.

三,支持Web版本吗?

支持,当前没有一个明确的时间表.

另,有任何问题欢迎提出.

名词

CIIP.Designer:是指CIIP的设计器,当前用于完成业务模型的构建过程.CIIP.Designer可以生成dll文件.供CIIP.Client来运行.

CIIP.Client:是指CIIP的运行时,当前仅有Windows版本,后面会逐步增加.

 

使用说明:

 

本次主要将设计功能与运行时分成了两个程序,当前的工作重心仍为Windows版本.

1.运行CIIP.Designer后,第一次启动时,将会提示没有项目,需要建立项目:
<br>
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180904113143790-2078829652.png)


2.按下确认后:
<br>
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180904113516172-1963524255.png)


3.输入名称如:CRM
进入系统后,点击 工具->数据初始化
<br>
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180904113648221-1142783986.png)


4.这里,可以看到下图所示.如果之前打开了用户业务列表,则此时需要按下刷新按钮.
<br>
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180904113821558-1167438408.png)


5.按下新建按钮:按下图提示填写相同的内容. 注:基类接口处可以按下空格弹出下拉框.
<br>
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180904114137804-1984351090.png)


6.按下生成按钮,会启动目标项目,要求输入用户名,这里键入默认用户名admin,无密码.
<br>
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180904115113417-1174592724.png)
 

7.确认后,开始建立默认的数据库,第一次时间稍长.成功后,找到如下界面.按下新建按钮.
<br>
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180904115337959-2065075769.png)


8.查找CIIP.Designer编译的dll文件.路径是在ciip.designer的安装路径下面,   %CIIP%\CRM\WIN\CRM.DLL

其中:%CIIP%是CIIPDesigenr的路径

CRM是建立项目时的名称

WIN是指Windows版本的程序

CRM.DLL是使用项目名称+.dll组成的.
<br>
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180904115552560-1385640971.png)


9.按下保存并关闭后,关掉CIIP.Client.

再CIIP.Designer中选择:<br>
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180904120032431-1995796218.png)


10.这次选择运行.输入用户名后,确认.
<br>
![](https://img2018.cnblogs.com/blog/669762/201809/669762-20180904120258685-1534004319.png)
 

这时,就看见了我们定义的Customer的增删改查功能了.

Q:为什么第8步中,需要关掉CIIP.Client然后再重新进入才行?

A:第一次使用时,只有CIIP.Client只有一个初始的环境,需要做一个初始设置.CIIP.Client是一个通用程序.需要个性化设置才能生效.配置完成后,配置文件是保存在目录下面的,以后不需要得新配置.只需直接点生成运行即可.
