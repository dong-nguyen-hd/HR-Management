<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container">
      <div class="avatar flex flex-center q-pt-md column">
        <q-avatar size="200px">
          <img :src="imageURL" />
        </q-avatar>
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
      <div class="userName q-mt-lg q-px-md">
        <q-input
          disable
          standout
          v-model="accountInfor.userName"
          type="text"
          prefix="User name:"
        >
          <template v-slot:prepend>
            <q-icon name="fas fa-user" />
          </template>
        </q-input>
      </div>
      <div class="name q-mt-sm q-px-md">
        <q-input
          standout
          clearable
          maxlength="500"
          v-model="accountInfor.name"
          type="text"
          prefix="Name:"
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
          standout
          clearable
          maxlength="500"
          v-model="accountInfor.email"
          type="email"
          prefix="Email:"
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
    };
  },
  methods: {
    ...mapActions("auth", ["useRefreshToken"]),
    ...mapMutations("auth", ["setInformation"]),

    async save() {
      
        let [resultUpdateImage, resultUpdateInfor]  = await Promise.all([
          this.requestUpdateImage(),
          this.requestSelfUpdate(),
        ]);

        if (resultUpdateImage.success && resultUpdateInfor.success) {
          this.setInformation(resultSelfUpdate.resource);
          this.mapInformation();

          console.log("Ok");

          this.$q.notify({
            type: "positive",
            message: "Successfully updated",
          });
        } else {
          this.$q.notify({
            type: "negative",
            message: resultSelfUpdate.message[0],
          });
        }
      // } catch {
      //   this.$q.notify({
      //     type: "negative",
      //     message: `Saving error!`,
      //   });
      //}
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
