var grid = {
    UinDataGrid: {
        columns: [
            { name: lang.label.name, data: 'Nick' },
            { name: lang.label.code, data: 'Uin' },
            { name: lang.label.createTime, data: 'CreateTime' },
            { name: lang.label.Province, data: 'Province' },
            { name: lang.label.Country, data: 'Country' },
            { name: lang.label.City, data: 'City' },
            
            { name: lang.label.operation, data:null }
        ],
        rows: [
            { data: 'Nick', map: 'Nick' },
            { data: 'Uin', map: 'Uin' },
            { data: 'CreateTime', map: 'CreateTime' }
        ]
    }
   
};
//存储缓存数据
var CacheHelper = {
    SetCacheItem: function (value, cacheName) {
        var json = value;
        if (typeof (string) != "string")
        {
            json = JSON.stringify(value);
        }
        window.localStorage[cacheName] = json;
    },
    GetCacheItem: function (cacheName) {
        var str = window.localStorage[cacheName];
        if (str == undefined) {
            return undefined;
        }
        return JSON.parse(str);
    },
    ClearCache: function () {
        window.localStorage.clear();
    }
}