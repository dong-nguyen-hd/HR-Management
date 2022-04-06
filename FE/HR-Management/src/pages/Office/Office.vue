<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container full-height full-width">
      <q-dialog v-model="showDelete" :persistent="deleteProcess">
        <q-card>
          <q-card-section class="row items-center">
            <span class="text-h6">Delete {{ getNameDelete }}?</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <span
              >This can’t be undone and it will be removed from database.</span
            >
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="deleteProcess"
              label="Cancel"
              color="primary"
              v-close-popup
            />
            <q-btn
              flat
              label="Delete"
              color="negative"
              @click="deleteOffice"
              :loading="deleteProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <q-dialog v-model="showOffice" :persistent="getPersistentOffice">
        <q-card style="width: 400px">
          <q-card-section class="row items-center">
            <span v-show="!showEdit" class="text-h6">Add Office</span>
            <span v-show="showEdit" class="text-h6">Edit Office</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <div class="q-mt-sm">
              <q-input
                ref="addCategoryOneRef"
                standout
                clearable
                maxlength="250"
                v-model="tempOfficeResource.name"
                type="text"
                label="Name:"
                :label-color="colorFocusCategory[0]"
                @focus="colorFocusCategory[0] = 'white'"
                @blur="colorFocusCategory[0] = ''"
                :rules="[(val) => !!val || 'Name is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="addCategoryTwoRef"
                standout
                clearable
                maxlength="250"
                v-model="tempOfficeResource.address"
                type="text"
                label="Address:"
                :label-color="colorFocusCategory[1]"
                @focus="colorFocusCategory[1] = 'white'"
                @blur="colorFocusCategory[1] = ''"
                :rules="[(val) => !!val || 'Address is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="officeProcess"
              label="Cancel"
              color="primary"
              v-close-popup
            />
            <q-btn
              v-show="!showEdit"
              flat
              label="Add"
              color="info"
              @click="addOffice"
              :loading="officeProcess"
            />
            <q-btn
              v-show="showEdit"
              flat
              label="Edit"
              color="info"
              @click="editOffice"
              :loading="officeProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <div class="table-component full-height full-width flex flex-center">
        <div class="q-mb-md full-width" style="height: 36px">
          <div class="full-width flex justify-end q-pr-md">
            <q-btn color="primary" label="New Office" @click="openAddOffice" />
          </div>
        </div>

        <q-table
          class="table-content q-mx-md"
          :rows="tempListOffice"
          :columns="headerTable"
          row-key="id"
          flat
          bordered
          dark
          :loading="loadingData"
          v-model:pagination="pagination"
          :rows-per-page-options="[5, 10, 15, 20]"
        >
          <template v-slot:header-cell-name="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthName}`">
                <q-input
                  @update:model-value="filterOffice"
                  dark
                  dense
                  standout
                  v-model="filterModel.name"
                  debounce="300"
                  input-class="text-right"
                  :label="labelNameFocus[0]"
                  :label-color="labelColorFocus[0]"
                  @focus="
                    labelColorFocus[0] = 'black';
                    labelNameFocus[0] = 'Search by name';
                    widthName = '200px';
                  "
                  @blur="
                    labelColorFocus[0] = 'white';
                    labelNameFocus[0] = props.col.label;
                    widthName = '130px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filterModel.name" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filterModel.name = '';
                        filterOffice();
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-address="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthAddress}`">
                <q-input
                  @update:model-value="filterOffice"
                  dark
                  dense
                  standout
                  v-model="filterModel.address"
                  debounce="300"
                  input-class="text-right"
                  :label="labelNameFocus[1]"
                  :label-color="labelColorFocus[1]"
                  @focus="
                    labelColorFocus[1] = 'black';
                    labelNameFocus[1] = 'Search by address';
                    widthAddress = '154px';
                  "
                  @blur="
                    labelColorFocus[1] = 'white';
                    labelNameFocus[1] = props.col.label;
                    widthAddress = '110px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filterModel.address" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filterModel.address = '';
                        filterOffice;
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:body-cell-action="props">
            <q-td :props="props">
              <div>
                <q-btn
                  style="width: 60px"
                  dense
                  color="white"
                  text-color="black"
                  label="Edit"
                  @click="openEditOffice(props.value)"
                />
              </div>
              <div class="q-mt-sm">
                <q-btn
                  style="width: 60px"
                  dense
                  color="negative"
                  label="delete"
                  @click="
                    idDelete = props.value;
                    showDelete = true;
                  "
                />
              </div>
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
import { mapActions } from "vuex";
import { useQuasar } from "quasar";
import { api } from "src/boot/axios";

export default defineComponent({
  name: "Office",

  data() {
    return {
      labelColorFocus: ["white", "white"],
      colorFocusCategory: ["", ""],
      labelNameFocus: ["Category", "Skills"],

      widthName: "130px",
      widthAddress: "110px",

      loadingData: false,

      showDelete: false,
      idDelete: null,
      deleteProcess: false,

      showOffice: false,
      officeProcess: false,

      idEdit: null,
      showEdit: false,

      tempOfficeResource: {
        name: "",
        address: "",
      },

      filterModel: {
        name: "",
        address: "",
      },

      pagination: {
        page: 1,
        rowsPerPage: 10,
      },

      tempListOffice: [],
      listOffice: [],
      headerTable: [
        {
          name: "index",
          label: "#",
          align: "center",
          field: "index",
        },
        {
          name: "name",
          align: "left",
          label: "Name",
          field: (row) => row.name,
        },
        {
          name: "address",
          align: "left",
          label: "Address",
          field: (row) => row.address,
        },
        {
          name: "action",
          align: "center",
          label: "Actions",
          field: (row) => row.id,
        },
      ],
    };
  },
  methods: {
    ...mapActions("auth", ["validateToken"]),

    async getAllOffice() {
      try {
        this.loadingData = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .get(`/api/v1/office`)
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
          this.listOffice = result.resource;

          this.listOffice.forEach((row, index) => {
            row.index = index + 1;
          });

          this.tempListOffice = this.listOffice;
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
    filterOffice(val) {
      console.log("TEST filter: " + JSON.stringify(this.filterModel));
      console.log("TEST func: " + JSON.stringify(val));
      if (this.filterModel.name && this.filterModel.address) {
        this.tempListOffice = this.listOffice.filter(
          (x) =>
            x.name.includes(this.filterModel.name.trim()) ||
            x.address.includes(this.filterModel.address.trim())
        );
      } else if (this.filterModel.name) {
        this.tempListOffice = this.listOffice.filter((x) =>
          x.name.includes(this.filterModel.name.trim())
        );
      } else if (this.filterModel.address) {
        this.tempListOffice = this.listOffice.filter((x) =>
          x.address.includes(this.filterModel.address.trim())
        );
      } else if (!this.filterModel.name && !this.filterModel.address) {
        this.tempListOffice = this.listOffice;
      }

      this.tempListOffice.forEach((row, index) => {
        row.index = index + 1;
      });
    },
    async deleteOffice() {
      try {
        this.deleteProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .delete(`/api/v1/office/${this.idDelete}`)
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
          await this.getAllOffice();

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
        this.deleteProcess = false;
        this.showDelete = false;
      }
    },
    async addOffice() {
      try {
        if (
          !this.$refs.addCategoryOneRef.validate() ||
          !this.$refs.addCategoryTwoRef.validate()
        )
          return null;

        this.officeProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .post(`/api/v1/office`, this.tempOfficeResource)
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
          this.tempListOffice.unshift(result.resource);
          this.listOffice.unshift(result.resource);

          this.tempListOffice.forEach((row, index) => {
            row.index = index + 1;
          });

          this.tempOfficeResource.name = "";
          this.tempOfficeResource.address = [];

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
        this.officeProcess = false;
      }
    },
    openAddOffice() {
      this.showEdit = false;

      this.tempOfficeResource.name = "";
      this.tempOfficeResource.address = "";

      this.showOffice = true;
    },
    async editOffice() {
      try {
        if (
          !this.$refs.addCategoryOneRef.validate() ||
          !this.$refs.addCategoryTwoRef.validate()
        )
          return null;

        this.officeProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .put(`/api/v1/office/${this.idEdit}`, this.tempOfficeResource)
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
          let index = this.listOffice.findIndex((x) => x.id == this.idEdit);
          let indexTemp = this.tempListOffice.findIndex(
            (x) => x.id == this.idEdit
          );

          // Original object
          this.listOffice[index].name = result.resource.name;
          this.listOffice[index].address = result.resource.address;
          // Temp object
          this.tempListOffice[indexTemp].name = result.resource.name;
          this.tempListOffice[indexTemp].address = result.resource.address;

          this.showOffice = false;

          this.$q.notify({
            type: "positive",
            message: "Successfully edited!",
          });
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.officeProcess = false;
      }
    },
    openEditOffice(id) {
      this.idEdit = id;
      this.showEdit = true;
      let tempOffice = this.tempListOffice.find((x) => x.id == id);

      this.tempOfficeResource.name = tempOffice.name;
      this.tempOfficeResource.address = tempOffice.address;

      this.showOffice = true;
    },
  },
  computed: {
    getNameDelete() {
      let tempCategory = this.listOffice.filter((x) => x.id == this.idDelete);

      let name = tempCategory[0]?.name;

      return name ? this.showName(name) : "";
    },
    getPersistentOffice() {
      return this.tempOfficeResource.name || this.tempOfficeResource.address
        ? true
        : false;
    },
  },
  async created() {
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([this.getAllOffice()]);
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

<style lang="scss">
.table-content {
  .q-table__top,
  .q-table__bottom,
  thead tr:first-child th {
    /* bg color is important for th; just specify one */
    background-color: $accent !important;
  }

  thead tr th {
    position: sticky;
    z-index: 99;
    font-size: 14px !important;
    font-family: Poppins-Medium !important;
  }

  tbody tr td {
    font-size: 14px !important;
  }

  thead tr:first-child th {
    top: 0;
  }

  /* this is when the loading indicator appears */
  &.q-table--loading thead tr:last-child th {
    /* height of all previous header rows */
    top: 0;
  }
}

/* Scroll bar */
::-webkit-scrollbar {
  width: 10px;
  height: 10px;
}

::-webkit-scrollbar-thumb {
  background: $accent;
}

::-webkit-scrollbar-corner {
  background: $grey;
}
</style>
