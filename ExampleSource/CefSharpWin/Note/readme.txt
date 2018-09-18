计划：

2018-09-10 
1.查询联系人，进行消息发送到票务监听主界面


2018-09-01
1.请求追踪，并将请求响应文件进行本地保存


执行顺序[20180901]
1：初始化登录
2.提取cookie
3.将cookie存储到容器中
4.获取联系人


涉及到的cookie项：
route
BIGipServerotn

JSESSIONID
RAIL_EXPIRATION
RAIL_DEVICEID

需要补充的项：
tk
BIGipServerpassport
ten_key
ten_js_key
RAIL_OkLJUJ	
BIGipServerpool_passport
tk<=> uamtk https://kyfw.12306.cn/passport/web/login


web:开启摄像头https://www.cnblogs.com/wangjiming/p/6801937.html
推测：
BIGipServerpassport由 https://kyfw.12306.cn/otn/resources/merged/login_UAM_js.js?scriptVersion=1.9097 生成

cookie 实例
JSESSIONID=86CCA97137202046AAF22A57798AECB8; 
tk=iCo7Hy8gVwsXZumzdhO-YAKvvDBYF4TWlthhdyiJrsiLWK41ij1210; 
RAIL_EXPIRATION=1536420789491; 
RAIL_DEVICEID=HXgTz3nUg7sHc0BE-YMnAuPsrUGndxefGxL2lhiPzauKQjIIfDnMANONL5vY7U25L2MF-fxPY5DZ_SryLR-AqKa2ATRq3UGJcjC920TefMJ46R17K4XAl35PxgcXLDBxq66-D58s9t816mQnlFSDXwYu-NIQdxou; 
route=9036359bb8a8a461c164a04f8f50b252; 
BIGipServerpool_passport=300745226.50215.0000; 
BIGipServerpassport=988283146.50215.0000; 
BIGipServerotn=467927562.38945.0000


返回tocken项的请求URL：https://kyfw.12306.cn/otn/uamauthclient


代理IP：
https://www.cnblogs.com/benbenfishfish/p/5830149.html
https://blog.csdn.net/zky0901/article/details/49305207
http://www.iphai.com/free/ng
http://www.ip181.com
https://blog.csdn.net/wuma0q1an/article/details/51312983
代理ip使用资料：
https://bbs.csdn.net/topics/391026270