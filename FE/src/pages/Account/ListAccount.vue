<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container full-height full-width">
      <q-dialog v-model="showDelete" :persistent="accountProcess">
        <q-card>
          <q-card-section class="row items-center">
            <span class="text-h6">{{ $t('delete') }} {{ getNameDelete }}?</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <span
              >{{ $t('confirmDelete') }}</span
            >
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="accountProcess"
              :label="$t('cancel')"
              color="primary"
              @click="closeModifyPopup"
            />
            <q-btn
              flat
              :label="$t('delete')"
              color="negative"
              @click="deleteAccount"
              :loading="accountProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <q-dialog v-model="show" :persistent="getPersistentAccount">
        <q-card style="width: 400px">
          <q-card-section class="row items-center">
            <span v-show="!showEdit" class="text-h6">{{ $t('addAccount') }}</span>
            <span v-show="showEdit" class="text-h6">{{ $t('editAccount') }}</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <div class="q-mt-sm">
              <q-input
                ref="oneRef"
                standout
                clearable
                maxlength="400"
                v-model="tempAccountResource.name"
                type="text"
                :label="$t('nameColon')"
                :label-color="colorFocusModifyPopup[0]"
                @focus="colorFocusModifyPopup[0] = 'white'"
                @blur="colorFocusModifyPopup[0] = ''"
                :rules="[(val) => !!val || 'Name is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="q-mt-sm" v-show="showInsert">
              <q-input
                ref="twoRef"
                standout
                clearable
                maxlength="250"
                v-model="tempAccountResource.userName"
                type="text"
                :label="$t('userNameDot')"
                :label-color="colorFocusModifyPopup[1]"
                @focus="colorFocusModifyPopup[1] = 'white'"
                @blur="colorFocusModifyPopup[1] = ''"
                :rules="[(val) => !!val || $t('userNameRequired')]"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="threeRef"
                standout
                clearable
                maxlength="250"
                v-model="tempAccountResource.password"
                type="text"
                :label="$t('pwdDot')"
                :label-color="colorFocusModifyPopup[2]"
                @focus="colorFocusModifyPopup[2] = 'white'"
                @blur="colorFocusModifyPopup[2] = ''"
                :rules="[(val) => !!val || $t('pwdRequired')]"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-select
                ref="fourRef"
                standout
                v-model="tempAccountResource.role"
                :options="arrRole"
                :label="$t('roleColon')"
                option-value="value"
                option-label="label"
                emit-value
                map-options
                options-selected-class="text-accent"
                :label-color="colorFocusModifyPopup[3]"
                @focus="colorFocusModifyPopup[3] = 'white'"
                @blur="colorFocusModifyPopup[3] = ''"
                :rules="[(val) => !!val || $t('roleRequired')]"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-select>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="fiveRef"
                standout
                clearable
                maxlength="400"
                v-model="tempAccountResource.email"
                type="text"
                :label="$t('emailColon')"
                :label-color="colorFocusModifyPopup[4]"
                @focus="colorFocusModifyPopup[4] = 'white'"
                @blur="colorFocusModifyPopup[4] = ''"
                :rules="[(val) => !!val || $t('emailRequired')]"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="accountProcess"
              :label="$t('cancel')"
              color="primary"
              @click="closeModifyPopup"
            />
            <q-btn
              v-show="!showEdit"
              flat
              :label="$t('btnAdd')"
              color="info"
              @click="saveInsert"
              :loading="accountProcess"
            />
            <q-btn
              v-show="showEdit"
              flat
              :label="$t('btnAdd')"
              color="info"
              @click="saveUpdate"
              :loading="accountProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <div
        class="table-component full-height full-width flex flex-center q-px-md"
      >
        <div class="new-item q-mb-md flex justify-end full-width">
          <!-- You can insert button here! -->
          <q-btn
            @click="openInsert"
            color="primary"
            :label="$t('newAccount')"
            unelevated
          />
        </div>

        <q-table
          class="table-content"
          :rows="listAccount"
          :columns="headerTable"
          row-key="id"
          flat
          bordered
          dark
          :loading="loadingData"
          v-model:pagination="pagination"
          :rows-per-page-options="[5, 10, 15, 20]"
          @request="getAccountWithFilter"
        >
          <template v-slot:header-cell-userName="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthOfUserName}`">
                <q-input
                  @update:model-value="getAccountWithFilter(false)"
                  dark
                  dense
                  standout
                  v-model="filter.userName"
                  input-class="text-right"
                  :label="labelNameFocus[0]"
                  :label-color="labelColorFocus[0]"
                  debounce="300"
                  @focus="
                    labelColorFocus[0] = 'black';
                    labelNameFocus[0] = $t('searchUserName');
                    widthOfUserName = '154px';
                  "
                  @blur="
                    labelColorFocus[0] = 'white';
                    labelNameFocus[0] = props.col.label;
                    widthOfUserName = '100px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filter.staffId" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filter.staffId = '';
                        getAccountWithFilter(false);
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-role="props">
            <q-th :props="props">
              <div :style="`width: ${widthOfRole};`">
                <q-select
                  @update:model-value="getAccountWithFilter(false)"
                  dense
                  standout
                  dark
                  clearable
                  v-model="filter.role"
                  :options="arrRole"
                  :label="props.col.label"
                  option-value="value"
                  option-label="label"
                  emit-value
                  map-options
                  options-selected-class="text-accent"
                  :label-color="labelColorFocus[1]"
                  @focus="
                    labelColorFocus[1] = 'black';
                    widthOfRole = '160px';
                  "
                  @blur="
                    labelColorFocus[1] = 'white';
                    widthOfRole = !filter.positionId ? '120px' : '160px';
                  "
                >
                </q-select>
              </div>
            </q-th>
          </template>

          <template v-slot:body-cell-avatar="props">
            <q-td :props="props">
              <q-avatar>
                <img :src="props.value" />
              </q-avatar>
            </q-td>
          </template>

          <template v-slot:body-cell-action="props">
            <q-td :props="props">
              <div v-show="props.value != -1">
                <q-btn
                  style="width: 60px"
                  dense
                  color="white"
                  text-color="black"
                  :label="$t('btnEdit')"
                  @click="openEdit(props.value)"
                />
              </div>
              <div class="q-mt-sm" v-show="preventDeleteSelfAccount(props.value)">
                <q-btn
                  style="width: 60px"
                  dense
                  color="negative"
                  :label="$t('btnDelete')"
                  @click="openDelete(props.value)"
                />
              </div>
            </q-td>
          </template>

          <template v-slot:body-cell-name="props">
            <q-td :props="props">
              {{ showName(props.value) }}
            </q-td>
          </template>

          <template v-slot:body-cell-lastActivity="props">
            <q-td :props="props">
              {{ convertDateTimeFormat(props.value) }}
            </q-td>
          </template>

          <template v-slot:body-cell-role="props">
            <q-td :props="props">
              <q-badge
                :color="getColorRole(props.value)"
                style="font-size: 14px"
                >{{ props.value }}
              </q-badge>
            </q-td>
          </template>

          <template v-slot:loading>
            <q-inner-loading showing color="primary" />
          </template>
        </q-table>
      </div>
    </div>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import { mapActions, mapGetters } from "vuex";
import { useQuasar, date } from "quasar";
import { api } from "src/boot/axios";
import MD5 from "crypto-js/md5";

export default defineComponent({
  name: "List Account",

  data() {
    return {
      labelColorFocus: ["white", "white"],
      labelNameFocus: [this.$t('userName'), this.$t('role')],

      colorFocusModifyPopup: [],

      widthOfUserName: "100px",
      widthOfRole: "120px",

      role: {
        admin: 1,
        editorQTNS: 2,
        editorQTDA: 3,
        viewer: 4,
        editorKT: 5,
      },
      arrRole: [
        {
          label: "admin",
          value: 1,
        },
        {
          label: "editor-qtns",
          value: 2,
        },
        {
          label: "editor-qtda",
          value: 3,
        },
        {
          label: "viewer",
          value: 4,
        },
        {
          label: "editor-kt",
          value: 5,
        },
      ],

      loadingData: false,

      showDelete: false,
      deleteObj: null,

      showEdit: false,
      editObj: null,
      showInsert: false,

      show: false,
      accountProcess: false,

      tempPwd: "1234@dongnguyen",
      tempAccountResource: {
        userName: "",
        password: "1234@dongnguyen",
        name: "",
        email: "",
        role: 4,
      },

      filter: {
        userName: null,
        role: null,
      },

      pagination: {
        page: 1,
        rowsPerPage: 10,
        rowsNumber: 10, // is total records

        firstPage: 1,
        lastPage: null,
        nextPage: null,
        previousPage: null,
        totalPages: 0,
      },

      listAccount: [],
      headerTable: [
        {
          name: "index",
          label: "#",
          align: "center",
          field: "index",
        },
        {
          name: "avatar",
          align: "center",
          label: "",
          field: (row) => row.avatar.thumbnail,
        },
        {
          name: "userName",
          align: "left",
          label: this.$t('userName'),
          field: "userName",
        },
        {
          name: "role",
          align: "center",
          label: this.$t('role'),
          field: "role",
        },
        {
          name: "name",
          align: "left",
          label: this.$t('name'),
          field: "name",
        },
        {
          name: "email",
          align: "left",
          label: this.$t('email'),
          field: "email",
        },
        {
          name: "lastActivity",
          align: "left",
          label: this.$t('lastActivity'),
          field: "lastActivity",
        },
        {
          name: "action",
          align: "center",
          label: this.$t('actions'),
          field: (row) => row.id,
        },
      ],
    };
  },
  methods: {
    ...mapActions("auth", ["useRefreshToken", "validateToken"]),

    openInsert() {
      this.tempAccountResource.name = "";
      this.tempAccountResource.userName = "";
      this.tempAccountResource.password = this.tempPwd;
      this.tempAccountResource.role = 4;
      this.tempAccountResource.email = "";

      this.showInsert = true;
      this.show = true;
    },
    async saveInsert() {
      try {
        if (
          !this.$refs.oneRef.validate() ||
          !this.$refs.twoRef.validate() ||
          !this.$refs.threeRef.validate() ||
          !this.$refs.fourRef.validate() ||
          !this.$refs.fiveRef.validate()
        )
          return null;

        this.accountProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        let payload = {
          name: this.tempAccountResource.name,
          userName: this.tempAccountResource.userName,
          password: MD5(this.tempAccountResource.password).toString(),
          role: this.tempAccountResource.role,
          email: this.tempAccountResource.email,
        }

        // Request API
        let result = await api
          .post(`/api/v1/account`, payload)
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
          if (this.listAccount.length >= this.pagination.rowsPerPage)
            this.listAccount.pop();
          this.listAccount.unshift(result.resource);

          this.listAccount.forEach((row, index) => {
            row.index = index + 1;
          });

          this.showInsert = false;
          this.show = false;

          this.$q.notify({
            type: "positive",
            message: "Successfully added!",
          });
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.accountProcess = false;
      }
    },
    closeModifyPopup(){
      this.showDelete = false;
      this.showEdit = false;
      this.showInsert = false;
      this.show = false;
    },
    async getAccount() {
      try {
        this.loadingData = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .get(`/api/v1/account?page=1&pageSize=10`)
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
          this.mappingPagination(result);
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.loadingData = false;
      }
    },
    async getAccountWithFilter(props) {
      try {
        this.loadingData = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        let { page, rowsPerPage } = props
          ? props.pagination
          : { page: 1, rowsPerPage: this.pagination.rowsPerPage };

        if (!this.filter.lastDay) this.filter.lastDay = null;

        // Request API
        let result = await api
          .post(
            `/api/v1/account/pagination?page=${page}&pageSize=${rowsPerPage}`,
            this.filter
          )
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
          this.mappingPagination(result);
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.loadingData = false;
      }
    },
    mappingPagination(resource) {
      this.listAccount = resource.resource;

      this.listAccount.forEach((row, index) => {
        row.index = index + 1;
      });

      this.pagination.page = resource.page;
      this.pagination.rowsPerPage = resource.pageSize;
      this.pagination.rowsNumber = resource.totalRecords;

      this.pagination.firstPage = resource.firstPage;
      this.pagination.lastPage = resource.lastPage;
      this.pagination.nextPage = resource.nextPage;
      this.pagination.previousPage = resource.previousPage;
      this.pagination.totalPages = resource.totalPages;
    },
    showName(text) {
      if (text.length > 20) return `${text.slice(0, 20)}...`;
      else return text;
    },
    openDelete(id) {
      this.deleteObj = this.listAccount.find((x) => x.id == id);
      this.showDelete = true;
    },
    async deleteAccount() {
      try {
        this.accountProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .delete(`/api/v1/account/${this.deleteObj.id}`)
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
          await this.getAccountWithFilter(false);

          this.$q.notify({
            type: "positive",
            message: "Successfully deleted!",
          });
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.accountProcess = false;
        this.showDelete = false;
      }
    },
    openEdit(id) {
      this.editObj = this.listAccount.find((x) => x.id == id);

      this.tempAccountResource.name = this.editObj.name;
      this.tempAccountResource.userName = this.editObj.userName;
      this.tempAccountResource.password = this.tempPwd;
      this.tempAccountResource.role = this.convertRoleStringToNumber(
        this.editObj.role
      );
      this.tempAccountResource.email = this.editObj.email;

      this.showEdit = true;
      this.show = true;
    },
    async saveUpdate() {
      try {
        if (
          !this.$refs.oneRef.validate() ||
          !this.$refs.twoRef.validate() ||
          !this.$refs.threeRef.validate() ||
          !this.$refs.fourRef.validate() ||
          !this.$refs.fiveRef.validate()
        )
          return null;

        this.accountProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        let payload = {
          name: this.tempAccountResource.name,
          password: MD5(this.tempAccountResource.password).toString(),
          userName: this.tempAccountResource.userName,
          role: this.tempAccountResource.role,
          email: this.tempAccountResource.email,
        }

        // Request API
        let result = await api
          .put(`/api/v1/account/${this.editObj.id}`, payload)
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
          let index = this.listAccount.findIndex(
            (x) => x.id == this.editObj.id
          );
          let numberIndex = this.listAccount[index].index;
          this.listAccount[index] = Object.assign({}, result.resource);
          this.listAccount[index].index = numberIndex;
          this.editObj = null;

          this.showEdit = false;
          this.show = false;

          this.$q.notify({
            type: "positive",
            message: "Successfully updated!",
          });
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.accountProcess = false;
      }
    },
    convertDateTimeFormat(dateTime, stringFormat = "YYYY-MM-DD hh:mm") {
      return date.formatDate(dateTime, stringFormat);
    },
    getColorRole(roleName) {
      if (roleName == "admin") return "green-10";
      if (roleName == "editor-qtns") return "red-10";
      if (roleName == "editor-qtda") return "purple-10";
      if (roleName == "editor-kt") return "cyan-10";
      if (roleName == "viewer") return "lime-10";
    },
    convertRoleStringToNumber(stringRole) {
      if (stringRole == "admin") return this.role.admin;
      if (stringRole == "editor-qtns") return this.role.editorQTNS;
      if (stringRole == "editor-qtda") return this.role.editorQTDA;
      if (stringRole == "editor-kt") return this.role.editorKT;
      if (stringRole == "viewer") return this.role.viewer;
    },
    preventDeleteSelfAccount(id){
      let idAccount = this.getInformation?.id;

      if(id == idAccount || id == -1) return false;

      return true;
    }
  },
  computed: {
    ...mapGetters("auth", ["getRole", "getInformation"]),

    getNameDelete() {
      return this.deleteObj ? this.showName(this.deleteObj.userName) : "";
    },
    getPersistentAccount() {
      return this.tempAccountResource.name ? true : false;
    },
  },
  async created() {
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await this.getAccount();
  },
  mounted() {
    const $q = useQuasar();
  },
});
</script>

<style lang="scss" scoped>
.container {
  position: relative;

  .table-component {
    position: relative;

    .table-content {
      /* height or max-height is important */
      height: 600px;
      width: 100%;
    }
  }
}
</style>
