let UTILS = {
    saveUserInfo: (user) => {
        //保存用户信息
        localStorage.setItem('user', JSON.stringify(user));
    },
    getUserToken: () => {
        let user = localStorage.getItem('user');
        if (user) {
            let { token } = JSON.parse(user);
            return token;
        }
        return '';

    },
    isLogined: () => {
        let user = localStorage.getItem('user');
        if (user) {
            return true;
        }
        return false;
    },
    getNowDate: () => {
        var date = new Date();
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }
        var currentDate = date.getFullYear() + "/" + month + "/" + strDate;
        return currentDate;
    }
};
export default UTILS;