//封装axios
import axios from "axios";
import { CONSTANT } from './const';
import { Notification } from 'element-ui';
import UTILS from './util';
//请求拦截
axios.interceptors.request.use(config => {
    if (!config.headers['Content-Type']) {
        config.headers['Content-Type'] = 'application/json; charset=UTF-8';
    }
    //添加token请求头
    let token = UTILS.getUserToken();
    config.headers['Authorization'] = 'Bearer ' + token;
    return config;
}, error => {
    return Promise.reject(error);
});

axios.interceptors.response.use(response => {
    return response;
}, error => {
    let response = error.response;
    console.log(response);
    //处理实体验证异常
    if (response.data.code === CONSTANT.RESULT_CODE.ENTITY_VALI_FAIL) {
        NotificationEntityError(response);
    }
    //处理通用的异常
    if (response.data.code === CONSTANT.RESULT_CODE.FAIL) {
        let error = response.data.data.error;
        Notification.error(error);
    }
    if (response.data.Code === CONSTANT.RESULT_CODE.NO_LOGIN || response.data.Code === CONSTANT.RESULT_CODE.NO_AUTH) {
        //如果没有登录或没有权限，跳转登录页并移除缓存
        Notification.error(response.data.Data.data);
        setTimeout(() => {
            window.location.href = CONSTANT.FRONT_URL;
            localStorage.removeItem('user');
        }, 2000);
    }
    return Promise.reject(error);
});

function NotificationEntityError(response) {
    let error = response.data.data.error;
    //实体异常一般都是数组
    error.forEach((data, index) => {
        setTimeout(() => {
            Notification.error(data.errorMessage);
        }, index * 100);
    });
}
export default axios;

