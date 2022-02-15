<template>
  <q-layout view="hHh lpR fFf">
    <q-header reveal class="bg-secondary text-white" style="height: 50px">
      <q-toolbar class="no-padding">
        <q-btn
          class="q-ml-md"
          dense
          flat
          round
          icon="menu"
          @click="toggleLeftDrawer"
        />
        <q-toolbar-title content-center>
          <q-avatar>
            <img src="../assets/images/logo-mono-white.svg" />
          </q-avatar>
        </q-toolbar-title>
        <div class="information q-pl-md" @click="isShowInfor = !isShowInfor">
          <div class="user-name q-mr-md">{{ getInformation.name }}</div>
          <div class="avatar q-mr-sm">
            <q-avatar size="36px">
              <img :src="getInformation.avatar" />
            </q-avatar>
          </div>
          <div class="role q-mr-sm q-px-sm">
            {{ getInformation.role }}
          </div>
        </div>

        <div class="information-menu" v-show="isShowInfor">
          <q-list dark separator>
            <q-item clickable v-ripple>
              <q-item-section avatar>
                <q-icon color="white" name="fas fa-user-edit" />
              </q-item-section>
              <q-item-section>Update personal information</q-item-section>
            </q-item>

            <q-item clickable v-ripple>
              <q-item-section avatar>
                <q-icon color="white" name="fas fa-key"/>
              </q-item-section>
              <q-item-section>Change password</q-item-section>
            </q-item>

            <q-item clickable v-ripple>
              <q-item-section avatar>
                <q-icon color="white" name="fas fa-sign-out-alt" />
              </q-item-section>
              <q-item-section>Log Out</q-item-section>
            </q-item>
          </q-list>
        </div>
      </q-toolbar>
    </q-header>

    <q-drawer
      show-if-above
      v-model="leftDrawerOpen"
      side="left"
      bordered
      behavior="desktop"
      class="bg-primary"
    >
      <!-- drawer content -->
      <div class="q-py-md bg-primary text-white">
        <q-list style="max-width: 318px">
          <q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon color="text" name="bluetooth" />
            </q-item-section>

            <q-item-section>Icon as avatar</q-item-section>
          </q-item>

          <q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon color="text" name="bluetooth" />
            </q-item-section>

            <q-item-section>Icon as avatar</q-item-section> </q-item
          ><q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon color="text" name="bluetooth" />
            </q-item-section>

            <q-item-section>Icon as avatar</q-item-section>
          </q-item>

          <q-separator class="bg-grey-7" />
        </q-list>
      </div>
    </q-drawer>

    <q-page-container @click="isShowInfor = isShowInfor ? false : false">
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { defineComponent } from "vue";
import { mapGetters } from "vuex";

export default defineComponent({
  name: "MainLayout",

  data() {
    return {
      leftDrawerOpen: false,
      isShowInfor: false,
    };
  },
  methods: {
    toggleLeftDrawer() {
      this.leftDrawerOpen = !this.leftDrawerOpen;
    },
    logOut() {
      console.log("Test: ", JSON.stringify(this.getInformation));
      //this.$store.dispatch("auth/logOut");
      //this.$router.replace("/");
    },
  },
  computed: {
    ...mapGetters("auth", ["isAuthenticated", "getInformation", "getToken"]),
  },
});
</script>

<style lang="scss" scoped>
.information {
  height: 50px;
  display: flex;
  flex-direction: row-reverse;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.4s;
  -webkit-transition: all 0.4s;
  -o-transition: all 0.4s;
  -moz-transition: all 0.4s;
  .avatar {
    border-radius: 50%;
    border: solid 2px #fff;
  }
  .user-name {
    font-size: 18px;
  }
  .role {
    width: auto;
    height: 20px;
    background-color: $green-9;
    border-radius: 5px;
  }
}

.information:hover {
  background-color: $hover;
}

.information-menu {
  background-color: $secondary;
  width: 291px;
  position: fixed;
  top: 50px;
  right: 0;
}
</style>
