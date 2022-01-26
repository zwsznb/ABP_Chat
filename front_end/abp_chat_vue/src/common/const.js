//表情
let emoji = ['😀', '😃', '😄', '😁', '😆', '😅', '🤣', '😂', '🙂',
    '🙃', '😉', '😊', '😇', '🥰', '😍', '🤩', '😘', '😗', '😚', '😙',
    '😋', '😛', '😜', '🤪', '😝', '🤑', '🤗', '🤭', '🤫', '🤔', '🤐',
    '🤨', '😐', '😑', '😶', '😏', '😒', '🙄', '😬', '🤥', '😌', '😔',
    '😪', '🤤', '😴', '😷', '🤒', '🤕', '🤢', '🤮', '🤧', '🥵', '🥶',
    '🥴', '😵', '🤯', '🤠', '🥳', '😎', '🤓', '🧐', '😕', '😟', '🙁',
    '☹️', '😮', '😯', '😲', '😳', '🥺', '😦', '😧', '😨', '😰', '😥',
    '😢', '😭', '😱', '😖', '😣', '😞', '😓', '😩', '😫', '🥱', '😤',
    '😡', '😠', '🤬', '🙏', '✍️', '💅', '🤳', '💪', '👶', '🧒', '👦',
    '👧', '🧑', '👱', '👨', '🧔', '👨‍🦰', '👨‍🦱', '👨‍🦳', '👨‍🦲', '👩', '👩‍🦰'];

let proxyPrefix = '/api/AbpChat';
let CONSTANT = {
    BASE_URL: 'http://localhost:5000',
    FRONT_URL: 'http://localhost:8080',
    WS: 'ws://localhost:5000',
    //比较统一的错误放到拦截器里面去处理
    RESULT_CODE: {
        SUCCESS: 200,
        ENTITY_VALI_FAIL: 10001,      //实体验证失败
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