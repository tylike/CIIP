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
重要的:在创建项目时,可以选择常用模块啦!
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180910221403980-1453306292.png)
第一步:启动CIIP.Designer
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180910220758972-1940858487.png)
第二步:创建Customer业务对象.
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180910220850476-1616014341.png)
第三步:点击生成按钮.出现登录界面,按下确定按钮.
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180910220923466-57481984.png)
好了,这就是结果,数据是为了方便理解,我录入的.
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180910221015233-204495841.png)
另外,还有一步创建子表:
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180910221618972-9361806.png)
生成看结果:多出了一个Order子表.
![](https://images2018.cnblogs.com/blog/669762/201809/669762-20180910221838834-598831172.png)
