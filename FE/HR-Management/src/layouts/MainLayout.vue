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
          <div class="user-name q-mr-md">{{ showName }}</div>
          <div class="avatar q-mr-sm">
            <q-avatar size="38px">
              <img :src="getInformation?.avatar?.thumbnail" />
            </q-avatar>
          </div>
          <div class="role q-mr-sm q-px-sm">
            {{ getInformation.role }}
          </div>
        </div>

        <div class="information-menu" v-show="isShowInfor">
          <q-list dark separator>
            <q-item
              @click="isShowInfor = false"
              clickable
              v-ripple
              to="/self-update"
              exact
              active-class="isActive"
            >
              <q-item-section avatar>
                <q-icon name="fas fa-user-edit" />
              </q-item-section>
              <q-item-section>Update personal information</q-item-section>
            </q-item>

            <q-item
              @click="isShowInfor = false"
              clickable
              v-ripple
              to="/change-password"
              exact
              active-class="isActive"
            >
              <q-item-section avatar>
                <q-icon name="fas fa-key" />
              </q-item-section>
              <q-item-section>Change password</q-item-section>
            </q-item>

            <q-item clickable v-ripple @click.once="logOutButton">
              <q-item-section avatar>
                <q-icon name="fas fa-sign-out-alt" />
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
      <div class="left-side-bar bg-primary">
        <q-list dark separator>
            <q-item
              clickable
              v-ripple
              to="/list-employee"
              exact
              active-class="isActive"
              class="q-pl-lg"
            >
              <q-item-section avatar>
                <q-icon name="summarize" />
              </q-item-section>
              <q-item-section>List Employee</q-item-section>
            </q-item>

            <q-item
            v-show="getRole == 'admin' || getRole == 'editor-qtda'"
              @click="isShowInfor = false"
              clickable
              v-ripple
              to="/project"
              exact
              active-class="isActive"
              class="q-pl-lg"
            >
              <q-item-section avatar>
                <q-icon name="business_center" />
              </q-item-section>
              <q-item-section>Project</q-item-section>
            </q-item>

            <q-item
              @click="isShowInfor = false"
              clickable
              v-ripple
              to="/category"
              exact
              active-class="isActive"
              class="q-pl-lg"
            >
              <q-item-section avatar>
                <q-icon name="category" />
              </q-item-section>
              <q-item-section>Category</q-item-section>
            </q-item>

            <q-item
              @click="isShowInfor = false"
              clickable
              v-ripple
              to="/position"
              exact
              active-class="isActive"
              class="q-pl-lg"
            >
              <q-item-section avatar>
                <q-icon name="badge" />
              </q-item-section>
              <q-item-section>Position</q-item-section>
            </q-item>

            <q-item
              v-show="getRole == 'admin'"
              @click="isShowInfor = false"
              clickable
              v-ripple
              to="/list-account-admin"
              exact
              active-class="isActive"
              class="q-pl-lg"
            >
              <q-item-section avatar>
                <q-icon name="account_circle" />
              </q-item-section>
              <q-item-section>Account</q-item-section>
            </q-item>

            <q-item
              v-show="getRole == 'editor-qtda'"
              @click="isShowInfor = false"
              clickable
              v-ripple
              to="/list-account-qtda"
              exact
              active-class="isActive"
              class="q-pl-lg"
            >
              <q-item-section avatar>
                <q-icon name="account_circle" />
              </q-item-section>
              <q-item-section>Account</q-item-section>
            </q-item>

          </q-list>
      </div>
    </q-drawer>

    <q-page-container @click="isShowInfor = false">
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { defineComponent } from "vue";
import { mapGetters, mapActions } from "vuex";

export default defineComponent({
  name: "MainLayout",

  data() {
    return {
      leftDrawerOpen: false,
      isShowInfor: false,
    };
  },
  methods: {
    ...mapActions("auth", ["logOut"]),

    toggleLeftDrawer() {
      this.leftDrawerOpen = !this.leftDrawerOpen;
    },
    async logOutButton() {
      try {
        this.isShowInfor = false;
        await this.logOut();
      } finally {
        this.$router.replace("/login");
      }
    },
  },
  computed: {
    ...mapGetters("auth", ["getInformation", "getRole"]),
    showName() {
      if (this.getInformation?.name?.length > 30)
        return `${this.getInformation?.name.slice(0, 30)}...`;
      else return this.getInformation?.name;
    },
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

.isActive {
  color: $accent;
}
</style>
