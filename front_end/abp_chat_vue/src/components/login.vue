<template>
  <div class="custom-flex-center login-box">
    <div class="custom-flex-column custom-flex-align">
      <img src="../assets/static/mainHeader.jpg" class="login-header margin-bottom10" />
      <el-input maxlength="20" v-model="user.userName" placeholder="请输入用户名" class="margin-bottom10"></el-input>
      <el-input
        maxlength="20"
        placeholder="请输入密码"
        v-model="user.password"
        show-password
        class="margin-bottom10"
      ></el-input>
      <el-radio-group v-model="radio1" class="margin-bottom10" @change="changeIsLogin">
        <el-radio-button label="登录"></el-radio-button>
        <el-radio-button label="注册"></el-radio-button>
      </el-radio-group>
      <el-button type="success" @click="toHome" style="width: 100%;" :loading="isLoading">确定</el-button>
    </div>
  </div>
</template>

<script>
import { CONSTANT } from "@/common/const";
import UTILS from "@/common/util";
import { Message } from "element-ui";
export default {
  data() {
    return {
      user: {
        userName: "",
        password: ""
      },
      isLogin: true,
      radio1: "登录",
      isLoading: false
    };
  },
  created() {},
  methods: {
    toHome() {
      let user = this.user;
      //切换请求路径
      let reqUrl = this.isLogin ? CONSTANT.API.LOGIN : CONSTANT.API.REGISTER;
      this.isLoading = true;
      this.$post(reqUrl, { ...user }).then(
        resp => {
          UTILS.saveUserInfo(resp.data);
          this.$router.push("/home");
          Message.success("登陆成功");
          this.isLoading = false;
        },
        err => {
          this.isLoading = false;
        }
      );
    },
    changeIsLogin(label) {
      if (label === "登录") {
        this.isLogin = true;
        return;
      }
      this.isLogin = false;
    }
  }
};
</script>

<style scoped>
.login-box {
  width: 300px;
  height: 350px;
  background: rgba(0, 0, 0, 0.5);
  border-radius: 10px;
  border: 1px solid rgba(0, 0, 0, 0.2);
}
.login-header {
  width: 80px;
  height: 80px;
  margin-bottom: 10px;
  border-radius: 50%;
  animation: rotate 6s linear infinite;
}
@keyframes rotate {
  0% {
     border-radius: 50%;
    transform: rotateZ(0deg);
  }
  100% {
     border-radius: 50%;
    transform: rotateZ(360deg);
  }
}
.margin-bottom10 {
  margin-bottom: 10px;
}
/deep/ .el-radio-button__inner {
  width: 115px;
}
</style>