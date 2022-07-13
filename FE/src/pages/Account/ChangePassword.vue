<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container">
      <div class="avatar flex flex-center q-pt-md column">
        <q-avatar size="200px">
          <img :src="getInformation.avatar.original" />
        </q-avatar>
      </div>
      <div class="old-password q-mt-lg q-px-md">
        <q-input
          ref="oldRef"
          standout
          lazy-rules
          maxlength="500"
          v-model="oldPassword"
          :type="showPassword[0] ? 'text' : 'password'"
          hide-bottom-space
          label="Old password:"
          :label-color="labelColorFocus[0]"
          @focus="labelColorFocus[0] = 'white'"
          @blur="labelColorFocus[0] = ''"
          :rules="[(val) => !!val || 'Old password is required']"
        >
          <template v-slot:prepend>
            <q-icon name="fas fa-lock" />
          </template>
          <template v-slot:append>
            <q-icon
              :name="showPassword[0] ? 'visibility_off' : 'visibility'"
              class="cursor-pointer"
              @click="showPassword[0] = !showPassword[0]"
            />
          </template>
        </q-input>
      </div>
      <div class="new-password q-mt-sm q-px-md">
        <q-input
          ref="newRef"
          standout
          lazy-rules
          maxlength="100"
          v-model="newPassword"
          :type="showPassword[1] ? 'text' : 'password'"
          hide-bottom-space
          label="New password:"
          :label-color="labelColorFocus[1]"
          @focus="labelColorFocus[1] = 'white'"
          @blur="labelColorFocus[1] = ''"
          :rules="[
            (val) => !!val || 'New password is required',
            (val) =>
              (val && val.length > 5) ||
              'Password must be more than 5 characters',
            (val) =>
              val != oldPassword ||
              'New password cannot be the same the old password',
          ]"
        >
          <template v-slot:prepend>
            <q-icon name="password" />
          </template>
          <template v-slot:append>
            <q-icon
              :name="showPassword[1] ? 'visibility_off' : 'visibility'"
              class="cursor-pointer"
              @click="showPassword[1] = !showPassword[1]"
            />
          </template>
        </q-input>
      </div>
      <div class="confirm-password q-mt-sm q-px-md">
        <q-input
          ref="confirmRef"
          standout
          lazy-rules
          maxlength="100"
          v-model="confirmPassword"
          :type="showPassword[1] ? 'text' : 'password'"
          hide-bottom-space
          label="Confirm password:"
          :label-color="labelColorFocus[2]"
          @focus="labelColorFocus[2] = 'white'"
          @blur="labelColorFocus[2] = ''"
          :rules="[
            (val) => !!val || 'Confirm password is required',
            (val) =>
              (val && val.length > 5) ||
              'Password must be more than 5 characters',
            (val) =>
              val == newPassword ||
              'Confirm password does not match the new password',
          ]"
        >
          <template v-slot:prepend>
            <q-icon name="password" />
          </template>
          <template v-slot:append>
            <q-icon
              :name="showPassword[1] ? 'visibility_off' : 'visibility'"
              class="cursor-pointer"
              @click="showPassword[1] = !showPassword[1]"
            />
          </template>
        </q-input>
      </div>
      <div class="btn-save flex flex-center q-pb-md">
        <q-btn
          :loading="loadingSave"
          :disable="loadingSave"
          @click="save"
          color="primary"
          label="Save"
          style="width: 100px"
        />
      </div>
    </div>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import { mapGetters, mapActions, mapMutations } from "vuex";
import { useQuasar } from "quasar";
import { api } from "src/boot/axios";
import MD5 from "crypto-js/md5";

export default defineComponent({
  name: "Change Password",

  data() {
    return {
      accountInfor: null,
      oldPassword: "",
      newPassword: "",
      confirmPassword: "",

      showPassword: [false, false],

      loadingSave: false,

      labelColorFocus: [],
    };
  },
  methods: {
    ...mapActions("auth", ["validateToken", "logOut"]),

    async save() {
      try {
        if (
          !this.$refs.oldRef.validate() ||
          !this.$refs.newRef.validate() ||
          !this.$refs.confirmRef.validate()
        ) {
          return null;
        }

        this.loadingSave = true;
        let isAuth = await this.validateToken();

        if (isAuth) {
          let result = await this.requestChangePassword();

          if (result.success) {
            this.$q.notify({
              type: "positive",
              message: "Successfully updated",
            });

            await this.logOut();

            // Set 3s to increase user experience
            setTimeout(() => {
              this.$router.replace("/login");
            }, 3000);
          } else {
            this.$q.notify({
              type: "negative",
              message: result.message[0],
            });
          }
        } else {
          this.$router.replace("/login");
        }
      } catch (ex) {
        this.$q.notify({
          type: "negative",
          message: `Saving error!`,
        });
      } finally {
        this.loadingSave = false;
      }
    },
    async requestChangePassword() {
      let payload = {
        oldPassword: MD5(this.oldPassword).toString(),
        newPassword: MD5(this.confirmPassword).toString(),
      };

      // Request API sefl-update
      return api
        .put(`/api/v1/account/change-password/${this.accountInfor.id}`, payload)
        .then((response) => {
          return response.data;
        })
        .catch(function (error) {
          // Checking if throw error
          if (error.response) {
            // Server response
            return error.response.data;
          } else {
            // Server not working
            let temp = { success: false, message: ["Server Error!"] };
            return temp;
          }
        });
    },
    mapInformation() {
      this.accountInfor = Object.assign({}, this.getInformation);
      this.imageURL = this.accountInfor.avatar.original;
    },
  },
  computed: {
    ...mapGetters("auth", ["getInformation"]),
  },
  created() {
    this.mapInformation();
  },
  mounted() {
    const $q = useQuasar();
  },
});
</script>

<style lang="scss" scoped>
.container {
  background-color: $accent;
  height: 600px;
  width: 460px;
  border-radius: 10px;
  position: relative;
  .btn-save {
    position: absolute;
    bottom: 0;
    width: 100%;
  }
}
</style>
