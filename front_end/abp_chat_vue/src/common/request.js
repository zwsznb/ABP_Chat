import axios from './http'
import { CONSTANT } from "@/common/const";

// axios.defaults.baseURL = CONSTANT.BASE_URL;
axios.defaults.withCredentials = true;

export function post(url, data = {}) {
    return new Promise((resolve, reject) => {
        axios.post(url, data)
            .then(response => {
                if (response.status === CONSTANT.RESULT_CODE.SUCCESS && response.data.code === CONSTANT.RESULT_CODE.SUCCESS) {
                    resolve(response.data)
                }
            }, (err) => {
                reject(err)
            })
    })
}

export function get(url, data = {}) {
    return new Promise((resolve, reject) => {
        axios.get(url, data)
            .then(response => {
                if (response.status === CONSTANT.RESULT_CODE.SUCCESS && response.data.code === CONSTANT.RESULT_CODE.SUCCESS) {
                    console.log(response.data);
                    resolve(response.data)
                }
            }, (err) => {
                reject(err)
            })
    })
}

