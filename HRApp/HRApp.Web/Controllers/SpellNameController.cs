using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.CommonData;
using Common.Data;
using HRApp.Model;
using IHRApp.Infrastructure;
using HRApp.Infrastructure;
using HRApp.IApplicationService;
using HRApp.ApplicationService;
using CommonHelperEntity;
namespace HRApp.Web.Controllers
{
    [Description("拼音维护")]
    public class SpellNameController : BaseController
    {
        //
        // GET: /SpellName/
        [Description("生僻字维护")]
        public ActionResult SpecialSpellNameDialog()
        {
            string test = @"中华人民共和国位于亚洲东部，太平洋西岸[1]  ，是工人阶级领导的、以工农联盟为基础的人民民主专政的社会主义国家[2]  。
1949年（己丑年）10月1日成立[3-4]  ，以五星红旗为国旗[5]  ，《义勇军进行曲》为国歌[6]  ，国徽内容包括国旗、天安门、齿轮和麦稻穗[7]  ，首都北京[8]  ，省级行政区划为23个省、5个自治区、4个直辖市、2个特别行政区[9]  ，是一个以汉族为主体民族，由56个民族构成的统一多民族国家，汉族占总人口的91.51%[10]  。
新中国成立后，随即开展经济恢复与建设[11]  ，1953年开始三大改造[12]  ，到1956年确立了社会主义制度，进入社会主义探索阶段[13]  。文化大革命之后开始改革开放，逐步确立了中国特色社会主义制度。[14] 
中华人民共和国陆地面积约960万平方公里，大陆海岸线1.8万多千米，岛屿岸线1.4万多千米，内海和边海的水域面积约470多万平方千米。海域分布有大小岛屿7600多个，其中台湾岛最大，面积35798平方千米。[1]  陆地同14国接壤，与6国海上相邻。[15-17] 
中国是四大文明古国之一[18-19]  ，有着悠久的历史文化，是世界国土面积第三大的国家[20-21]  ，世界第一大人口国，与英、法、美、俄并为联合国安理会五大常任理事国。中国是世界第二大经济体[22-23]  ，世界第一贸易大国[24]  ，世界第一大外汇储备国[25]  ，世界第一大钢铁生产国和世界第一大农业国[26-27]  ，世界第一大粮食总产量国以及世界上经济成长最快的国家之一[28-29]  。第二大吸引外资国[30]  ，还是世界许多国际组织的重要成员[31]  ，被认为是潜在超级大国之一。[32-33] 
中华人民共和国拥有最丰富的世界文化遗产和自然人文景点，是世界旅游大国之一.正则表达式匹配标点符号.甴曱 youyue  
月经期间的饮食
女性的益颜健体饮食调养的原则之一，就是与月经周期变化相吻合的;周期饮食;。这也是男女间益颜健体饮食调养中最主要的一个不同点。
不少女性，在月经来潮的前几天(月经前期)会有一些不舒服的症状，如抑郁、忧虑、情绪紧张、失眠、易怒、烦躁不安、疲劳等。一般认为，这与体内雌激素、孕激素的比例失调有关。此时，女性应选择既有益肤美容作用，又能补气、疏肝、调节不良情绪的食品、药品，如卷心菜、柚子、瘦猪肉、芹菜、粳米、鸭蛋、炒白术、淮山药、苡米、百合、金丝瓜、冬瓜、海带、海参、胡萝卜、白萝卜、胡桃仁、黑木耳、蘑菇等。
在月经来潮时，可出现食欲差、腰酸、疲劳等症状。此时，宜选用既有益肤美容作用，又对;经水之行;有益的食品、药品。宜选用的食品与药品有;羊肉、鸡肉、红枣、豆腐皮、苹果、薏苡仁、牛肉、牛奶、鸡蛋、红糖、益母草、当归、熟地、桃花等。
古人赵之弼云：;经水之行，常用热而不用寒，寒则止留其血，使浊秽不尽，带淋瘕满，所由作矣。;因此，在月经期间，许多在平时有很好的益肤美容作用的食品也应禁食，如梨子、香蕉、荸荠、石耳、石花、菱角、冬瓜、芥蓝、黑木耳、兔子肉、大麻仁等。
如上述，月经来潮时，要丢失一部分血液。血液的主要成分有血浆蛋白、钾、铁、钙、镁等无机盐。这就是说，每次来月经都会丢失一部分蛋白质与无机盐。因此，从原则上讲，月经干净之后的1-5天内(月经期后)，应补充蛋白质、矿物质等营养物质及用一些补血药。在此期间可选用既可益肤美容又有补血活血的作用的食品与药品有：牛奶、鸡蛋、鹌鹑蛋、牛肉、羊肉、猪胰、芡实、菠菜、樱桃、龙眼肉、荔枝肉、胡萝卜、苹果、当归、红花、桃花、熟地、黄精等。
蟑螂的意思;;马（马）騳(dú) 骉（驫）biao;虎 虤(yán);;龙（龙） 龖(dá) 龘 dá 四个龙（最新出版的修订本《汉语大词典》收录了4个繁体龙字合并的字，共有64画，是我国汉字中笔画最多的。）zhe;;牛 牪yàn 犇ben;鹿 麤cū;鱼（鱼） 鱻xiān;羊 羴shan;;犬 两个犬（待考，据说表示狗打架的意思）猋：biao;虫 虫chong;;贝 赑（贔）bi;隹zhui1 雥za2 雦ji2;;五行：;金 两个金（读Pian, 一种金属乐器） 鑫 四个金（计算机没法显示）;木 林 森 四个木（计算机没法显示）读gua4;水 沝 zhui3 淼 四个水（计算机没法显示）;火 炎yan4 炏yan2 焱 燚yi4;土 圭 垚yao2;;人相关：;;口 吕 回 吅xuan1 品 四个口（计算机没法显示）ji2;舌 舙;耳 聑 tiē 或zhe2 聂nie4 同聂;手 掱pa2;目 瞐mo4;毛 毳（cuì）;心 惢suo3;;人 从 仌bing 众 四个人（计算机没法显示）;女 奻nuan2 奸jian1;士 壵zhuang4;言 誩 jing4 譶ta4;呆 槑mei2;力 劦xie2;子 孖ma1 孨zhuan3;直 矗chu4;小 尛mo2;大 夶 bi3 三个大（太的俗字）;香 馫xin1 馨;;自然：;日 昌 昍xuan1 晶 晿cheng1 四个日（计算机无法显示）hua2;月 朋 三个月liao2（无法显示） 朤(lǎng);有道家对联：日昍晶hua通天地，月朋liao朤定乾坤;;云 两个云du4 三个云gu1;山 出 两个山左右并列cha2 不=屾shen 三个山shen1;有道家对联：gu du 云中神仙府，ya cha 山上道人家;;风 飍xiu1;雷 靐bing4 四个雷（计算机无法显示）beng4;泉 灥quan2或4;车 轰hong1轰;田 畕jiang1疆 畾lei3;刀 刕li2作姓;匕 比 两个比（电脑无法显示）;石 砳le4 磊 四个石（待考）;竹 四个竹（电脑无法显示，待考）;;其他：;飞 飝 fei1飞;原 厵yuan2源;又 双 叒ruo 4 叕zhuo2;屮che2 艸cao3 芔hui4卉 茻mang3;止 歮se4;白 皛xiao3;工 四个工（电脑无法显示）zhan3或zhan4章太炎的女儿名字有这个字;吉 喆zhe2哲 嚞zhe2哲;厶si1 厸lin2 厽lei3 四个厶（电脑无法显示）;;双叠字除上面之外，还有很多，整理全又难度。部分如下：;;左右：双 比 从 弱 林 竹 兢 祘 朋 聑 喆 囍 誩 赫 羽 屾 甡 兟 槑 棘 歰 孖 艸 砳 牪 龖 騳 豩bin 丝丽竝bing4 弜jiang4 臸zhi1 吅xuan1 斦yin2 秝li2;厸lin2 皕bi4 夶bi3 册 卌xi4 沝 竸 昍 奻 珏jue2（同珏）兹cī xuán zī 兹;踀chù chuò 兓xīn jin jīn賏yìng yīng 丛;;上下：二 哥 吕 圭 多 炎 昌 出 畕 刍 爻 枣 仌 亖si4 戋 串 畺;;内外：回闁bao1;;三个字组成的汉字：;三个金念鑫（xīn）;三个木念森（sēn）;三个水念淼（miǎo）;三个火念焱（yàn）;三个土念垚（yáo）;三个日念晶（jīng）;三个石念磊（lěi）;三个人念众（zhòng）;三个口念品（pǐn）;三个牛念犇（bēn）;三个手念掱（pá）;三个目念瞐（mò）;三个田念畾（lěi）;三个马念骉（biāo）;三个羊念羴（shān）;三个犬念猋（biāo）;三个鹿念麤（cū）;三个鱼念鱻（xiān）;三个贝念赑（bì）;三个力念劦（xié）;三个手念毳（cuì）;三个耳念聂（niè）;三个车念轰（hōng）;三个直念矗（chù）;三个龙念龘（tà、dá）;三个原念厵（yuán）;三个雷念靐（bìng）;三个飞念飝（fēi）;三个刀念刕（lí）;三个又念叒（ruò）;三个士念壵（zhuàng）;三个小念尛（mó）;三个子念孨（zhuǎn）;三个止念歮（sè）;三个风念飍（xiū）;三个隼念雥（zá）;三个吉念嚞（zhé）;三个言念譶（tà）;三个舌念舙（qì）;三个香念馫（xīn）;三个泉念灥（xún）;三个心念惢（suǒ）;三个白念皛（xiǎo）";// "正则@表达式&*只能输入中,；:=+文和字母zhongguo1949垚";
            CommonCallService.TextConvertSpellName(test, InitAppSetting.LogicDBConnString);
            return View();
        }
        [Description("保存生僻字")]
        public JsonResult SaveSpecialSpellName(NodeRequestParam param) 
        {
            JsonData json = new JsonData();
            if (string.IsNullOrEmpty(param.Name))
            { 
           
            }
            ISpecialSpellNameRepository spellRepository = new SpecialSpellNameRepository() { SqlConnString = InitAppSetting.LogicDBConnString };
            ISpecialSpellNameService appSetService = new SpecialSpellNameService(spellRepository);
            json = appSetService.Add(new SpecialSpellName()
            {
                Name = param.Name[0].ToString(),//只读取第一个字符
                Code = param.Code
            });
            return Json(json);
        }
        [HttpPost]
        [Description("查询生僻字列表")]
        public JsonResult QuerySpecialSpellNames(QueryRequestParam param) 
        {
            JsonData json = new JsonData();
            return Json(json);
        }
    }
}
