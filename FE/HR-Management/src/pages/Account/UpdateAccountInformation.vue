<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container">
      <div class="avatar flex flex-center q-pt-md column">
        <q-avatar size="200px">
          <img :src="imageURL" />
        </q-avatar>
        <div class="flex row no-wrap" style="width: 300px">
          <div class="flex flex-center q-mr-md q-pt-sm">
            <q-btn
              round
              unelevated
              color="primary"
              size="10px"
              icon="restart_alt"
              @click="resetAvatar"
              ><q-tooltip anchor="top middle" self="bottom middle"
                >Reset Avatar</q-tooltip
              >
            </q-btn>
          </div>
          <q-file
            class="q-mt-sm cursor-pointer"
            dense
            clearable
            standout
            tabindex="-1"
            input-style="width: 150px;"
            max-total-size="5242880"
            :display-value="
              imageFile ? 'Image is uploaded' : 'Upload image here!'
            "
            v-model="imageFile"
            accept=".jpg, .png, .gif, .bmp, image/*"
            @update:model-value="previewImage"
            @clear="clearTempImage"
            @rejected="onRejected"
          >
            <template v-slot:prepend>
              <q-icon name="attach_file" />
            </template>
          </q-file>
        </div>
      </div>
      <div class="userName q-mt-lg q-px-md">
        <q-input
          disable
          standout
          v-model="accountInfor.userName"
          type="text"
          label="User name:"
          :label-color="labelColorFocus[0]"
          @focus="labelColorFocus[0] = 'white'"
          @blur="labelColorFocus[0] = ''"
        >
          <template v-slot:prepend>
            <q-icon name="fas fa-user" />
          </template>
        </q-input>
      </div>
      <div class="name q-mt-sm q-px-md">
        <q-input
          ref="nameRef"
          standout
          clearable
          maxlength="500"
          v-model="accountInfor.name"
          type="text"
          label="Name:"
          :label-color="labelColorFocus[1]"
          @focus="labelColorFocus[1] = 'white'"
          @blur="labelColorFocus[1] = ''"
          :rules="[(val) => !!val || 'Name is required']"
          hide-bottom-space
        >
          <template v-slot:prepend>
            <q-icon name="fas fa-user" />
          </template>
        </q-input>
      </div>
      <div class="email q-mt-sm q-px-md">
        <q-input
          ref="emailRef"
          standout
          clearable
          maxlength="500"
          v-model="accountInfor.email"
          type="email"
          label="Email:"
          :label-color="labelColorFocus[2]"
          @focus="labelColorFocus[2] = 'white'"
          @blur="labelColorFocus[2] = ''"
          :rules="[(val) => !!val || 'Email is required']"
          hide-bottom-space
        >
          <template v-slot:prepend>
            <q-icon name="mail" />
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

export default defineComponent({
  name: "Update Personal Information",

  data() {
    return {
      accountInfor: null,
      imageURL: null,
      imageFile: null,

      loadingSave: false,

      labelColorFocus: [],
    };
  },
  methods: {
    ...mapActions("auth", ["validateToken"]),
    ...mapMutations("auth", ["setInformation"]),

    async save() {
      try {
        if (!this.$refs.nameRef.validate() || !this.$refs.emailRef.validate()) {
          return null;
        }

        this.loadingSave = true;
        let isAuth = await this.validateToken();

        if (isAuth) {
          let [resultUpdateImage, resultUpdateInfor] = await Promise.all([
            this.requestUpdateImage(),
            this.requestSelfUpdate(),
          ]);

          if (resultUpdateInfor.success) {
            this.setInformation(resultUpdateInfor.resource);
            this.mapInformation();
          } else {
            this.$q.notify({
              type: "negative",
              message: resultUpdateInfor.message[0],
            });

            return null;
          }

          if (!resultUpdateImage.success && this.imageFile) {
            this.imageURL = this.getInformation.avatar.original;
            this.imageFile = null;
            this.$q.notify({
              type: "negative",
              message: resultUpdateImage.message[0],
            });
            return null;
          } else if (resultUpdateImage.success) {
            resultUpdateInfor.resource.avatar =
              resultUpdateImage.resource.avatar;
            this.setInformation(resultUpdateInfor.resource);
            this.mapInformation();
          }

          this.$q.notify({
            type: "positive",
            message: "Successfully updated",
          });
        } else {
          this.$router.replace("/login");
        }
      } catch {
        this.$q.notify({
          type: "negative",
          message: `Saving error!`,
        });
      } finally {
        this.loadingSave = false;
      }
    },
    async resetAvatar() {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .put(`/api/v1/account/reset-avatar/${this.accountInfor.id}`)
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

      if (result.success) {
        let path = result.resource.avatar;
        this.imageFile = null;
        let infor = Object.assign({}, this.getInformation);
        infor.avatar = path;
        this.setInformation(infor);
        this.mapInformation();
        this.$q.notify({
          type: "positive",
          message: "Successfully",
        });
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async requestUpdateImage() {
      let fd = new FormData();
      fd.append("image", this.imageFile);

      // Request API update image
      return api
        .put(`/api/v1/account/image/${this.accountInfor.id}`, fd)
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
    async requestSelfUpdate() {
      let payload = {
        name: this.accountInfor.name,
        email: this.accountInfor.email,
      };

      // Request API sefl-update
      return api
        .put(`/api/v1/account/self-update/${this.accountInfor.id}`, payload)
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
    async previewImage() {
      this.imageURL = this.imageFile
        ? URL.createObjectURL(this.imageFile)
        : null;
    },
    async clearTempImage() {
      this.imageURL = this.accountInfor.avatar.original;
    },
    onRejected(rejectedEntries) {
      this.$q.notify({
        type: "negative",
        message: `Image size must lower than 5 MB`,
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
