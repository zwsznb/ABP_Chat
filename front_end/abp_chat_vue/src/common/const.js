//่กจๆ
let emoji = ['๐', '๐', '๐', '๐', '๐', '๐', '๐คฃ', '๐', '๐',
    '๐', '๐', '๐', '๐', '๐ฅฐ', '๐', '๐คฉ', '๐', '๐', '๐', '๐',
    '๐', '๐', '๐', '๐คช', '๐', '๐ค', '๐ค', '๐คญ', '๐คซ', '๐ค', '๐ค',
    '๐คจ', '๐', '๐', '๐ถ', '๐', '๐', '๐', '๐ฌ', '๐คฅ', '๐', '๐',
    '๐ช', '๐คค', '๐ด', '๐ท', '๐ค', '๐ค', '๐คข', '๐คฎ', '๐คง', '๐ฅต', '๐ฅถ',
    '๐ฅด', '๐ต', '๐คฏ', '๐ค ', '๐ฅณ', '๐', '๐ค', '๐ง', '๐', '๐', '๐',
    'โน๏ธ', '๐ฎ', '๐ฏ', '๐ฒ', '๐ณ', '๐ฅบ', '๐ฆ', '๐ง', '๐จ', '๐ฐ', '๐ฅ',
    '๐ข', '๐ญ', '๐ฑ', '๐', '๐ฃ', '๐', '๐', '๐ฉ', '๐ซ', '๐ฅฑ', '๐ค',
    '๐ก', '๐ ', '๐คฌ', '๐', 'โ๏ธ', '๐', '๐คณ', '๐ช', '๐ถ', '๐ง', '๐ฆ',
    '๐ง', '๐ง', '๐ฑ', '๐จ', '๐ง', '๐จโ๐ฆฐ', '๐จโ๐ฆฑ', '๐จโ๐ฆณ', '๐จโ๐ฆฒ', '๐ฉ', '๐ฉโ๐ฆฐ'];

let proxyPrefix = '/api/AbpChat';
let CONSTANT = {
    BASE_URL: 'http://localhost:5000',
    FRONT_URL: 'http://localhost:8080',
    WS: 'ws://localhost:5000',
    //ๆฏ่พ็ปไธ็้่ฏฏๆพๅฐๆฆๆชๅจ้้ขๅปๅค็
    RESULT_CODE: {
        SUCCESS: 200,
        ENTITY_VALI_FAIL: 10001,      //ๅฎไฝ้ช่ฏๅคฑ่ดฅ
        FAIL: 10002,
        NO_LOGIN: 10011,
        NO_AUTH: 10012
    },
    API: {
        //Account
        LOGIN: proxyPrefix + '/Account/login',
        REGISTER: proxyPrefix + '/Account/Register',
        FIND_USER: proxyPrefix + '/Account/FindUsers',
        LOGIN_OUT: proxyPrefix + '/Account/Loginout',
        CHANGE_PASS: proxyPrefix + '/Account/ChangePass',
        //Invitation
        INVITATION_COUNT: proxyPrefix + '/FriendInvitation/FindNewInvitation',
        READ_INVITATION: proxyPrefix + '/FriendInvitation/ReadAllInvitation',
        REFUSE_INVITATION: proxyPrefix + '/FriendInvitation/RefusedInvitation',
        FIND_ALL_INVITATION: proxyPrefix + '/FriendInvitation/FindAllInvitation',
        ADD_INVITATION: proxyPrefix + '/FriendInvitation/AddInvitaion',
        //Friend
        ADD_FRIEND: proxyPrefix + '/FriendManager/AddFriend',
        FIND_FRIEND: proxyPrefix + '/FriendManager/FindFriends',
        //chat
        FIND_ALL_MESSAGE_COUNT: proxyPrefix + '/ChatManager/FindAllNewMessage',
        SEND_MSG: proxyPrefix + '/ChatManager/SendMsg',
        FIND_ALL_NOREAD_MESSAGE: proxyPrefix + '/ChatManager/FindMsgsAndReadByFriend',
        READ_MESSAGE_BY_ID: proxyPrefix + '/ChatManager/ReadSingleMessage'
    }
};

export { emoji, CONSTANT };