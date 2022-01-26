<template>
  <el-dialog title="修改密码" :visible.sync="modifyPasswordDialogShow.show" width="20%">
    <el-row style="margin-bottom: 10px">
      <el-col>
        <el-input v-model="passsword.oldPassword" placeholder="旧密码"></el-input>
      </el-col>
    </el-row>
    <el-row>
      <el-col>
        <el-input v-model="passsword.newPassword" placeholder="新密码"></el-input>
      </el-col>
    </el-row>
    <div slot="footer" class="dialog-footer">
      <el-button @click="modifyPasswordDialogShow.show = false">取 消</el-button>
      <el-button type="primary" @click="submit">确 定</el-button>
    </div>
  </el-dialog>
</template>

<script>
import { CONSTANT } from "@/common/const";
export default {
  props: {
    modifyPasswordDialogShow: {
      show: false
    }
  },
  data() {
    return {
      passsword: {
        oldPassword: "",
        newPassword: ""
      }
    };
  },
  methods: {
    submit() {
      if (
        this.passsword.oldPassword === "" ||
        this.passsword.newPassword === ""
      ) {
        return;
      }
      console.log("dddd");
      this.$post(CONSTANT.API.CHANGE_PASS, {
        oldPassword: this.passsword.oldPassword,
        newPassword: this.passsword.newPassword
      }).then(resp => {
        //重新登录
        localStorage.removeItem("user");
        this.$router.push("/login");
      });
    }
  }
};
</script>

<style lang="scss" scoped>
</style>