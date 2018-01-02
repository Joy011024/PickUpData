var grid = {
    outSchoolInfo: {
        columns: [
            { name: lang.label.name, data: 'Name' },
            { name: lang.label.code, data: 'Code' },
            { name: lang.label.operation, data:null }
        ],
        rows: [
            { data: 'Name', map: 'Name' },
            { data: 'Code', map: 'Code' }
        ]
    },
    abroadStudent: {
        columns: [
            {
                name: '<span> <input type="checkbox" class="CkAllNode" style="width:80px;"/></span>', data: 'Code',
                render: function (val, display, row) {
                    return '<span> <input type="checkbox" class="CkAllNode" id="' + val + '" val="'+val+'"/></span>';
                }
            },
            { name: lang.label.name, data: 'StuName' },
            { name: lang.label.code, data: 'StuCode' },
            { name: lang.label.country, data: 'Native' },
            { name: lang.label.nation, data: 'Nation' },
            { name: lang.label.schoolName, data: 'SchoolId' },
            {
                name: lang.label.registerData, data: 'RegisterDate', render: function (val, display, row) {
                    return ChangeDateFormat(val);
                }
            },
            { name: lang.label.idCard, data: 'IdCard' },
            { name: lang.label.idType, data: 'IdType' },
            { name: lang.label.idTypeNo, data: 'IdTypeNo' },
            { name: lang.label.license, data: 'LicenseNo' },
            { name: lang.label.mobileNo, data: 'MobileNo' },
            {
                name: lang.label.registerResult, data: 'WcfId'
            }
        ],
        rows: [
            { data: 'Id', map: 'Code' },
            { data: 'Name', map: 'StuName' },
            { data: 'WcfId', map: 'WcfId' },
            { data: 'StuCode', map: 'StuCode' },
            { data: 'Education', map: 'Education' },
            { data: 'Native', map: 'Native' },
            { data: 'Nation', map: 'Nation' }
        ]
    },
    AviatorPay: {//训练费用
        columns: [
            {
                name: lang.label.personType, data: 'Employer',
                render: function (val, display, row) {

                }
            }
        ]
    },
    DutyGroupMember: {
        columns: [
            { name: lang.label.dutyGroupName, data: 'DutyGroupName' },
            { name: lang.label.name, data: 'MemberName' }
        ]
    },
    MemberWorkSchedule: {
        columns: [
             { name: lang.label.dutyGroupName, data: 'DutyGroupName' },
             { name: lang.label.name, data: 'MemberName' },
             { name: lang.label.beginTime, data: 'WorkBeginTime' },
             { name: lang.label.endTime, data: 'WorkEndTime' }
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