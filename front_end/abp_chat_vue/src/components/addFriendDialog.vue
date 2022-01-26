<template>
  <div>
    <el-dialog title="添加好友" :visible.sync="addFriendDialogShow.show" width="20%">
      <el-row>
        <el-col :span="13">
          <el-input size="mini" v-model="searchFriendInput" placeholder="请输入好友昵称"></el-input>
        </el-col>
        <el-col :span="5" :offset="1">
          <el-button size="mini" type="primary" @click="findUser">搜索</el-button>
        </el-col>
      </el-row>

      <el-table :data="userList" style="widht: 100%;" v-loading="loading">
        <el-table-column property="nickName" label="昵称"></el-table-column>
        <el-table-column label="操作">
          <template slot-scope="scope">
            <el-button size="mini" type="success" @click="addInvitation(scope)">添加</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-dialog>
  </div>
</template>

<script>
import { CONSTANT } from "@/common/const";
export default {
  props: {
    addFriendDialogShow: {
      show: false
    }
  },
  data() {
    return {
      userList: [],
      searchFriendInput: "",
      loading: false
    };
  },
  methods: {
    findUser() {
      if (this.searchFriendInput === "") {
        return;
      }
      this.loading = true;
      this.$get(
        CONSTANT.API.FIND_USER + `?searchStr=${this.searchFriendInput}`
      ).then(resp => {
        this.userList = [];
        let searchResult = resp.data.forEach(data => {
          this.userList.push({ userId: data.id, nickName: data.userName });
        });
        this.loading = false;
      });
    },
    //好友邀请
    addInvitation(scope) {
      this.$post(CONSTANT.API.ADD_INVITATION, {
        friendId: scope.row.userId
      }).then(resp => {
        console.log(resp);
      });
    }
  }
};
</script>

<style lang="scss" scoped>
</style>