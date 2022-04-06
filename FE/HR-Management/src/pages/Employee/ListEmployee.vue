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
              @click="deleteEmployee"
              :loading="deleteProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <div class="table-component full-height full-width flex flex-center">
        <div class="new-item q-mb-md q-pr-md flex justify-end full-width">
          <q-btn to="/new-employee" color="primary" label="New employee" />
        </div>

        <q-table
          class="table-content q-mx-md"
          :rows="listEmployee"
          :columns="headerTable"
          row-key="id"
          flat
          bordered
          dark
          :loading="loadingData"
          v-model:pagination="pagination"
          :rows-per-page-options="[5, 10, 15, 20]"
          @request="getEmployeeWithFilter"
        >
          <template v-slot:header-cell-staffId="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthOfStaffId}`">
                <q-input
                  @update:model-value="getEmployeeWithFilter(false)"
                  dark
                  dense
                  standout
                  v-model="filter.staffId"
                  input-class="text-right"
                  :label="labelNameFocus[0]"
                  :label-color="labelColorFocus[0]"
                  debounce="300"
                  @focus="
                    labelColorFocus[0] = 'black';
                    labelNameFocus[0] = 'Search by staff id';
                    widthOfStaffId = '154px';
                  "
                  @blur="
                    labelColorFocus[0] = 'white';
                    labelNameFocus[0] = props.col.label;
                    widthOfStaffId = '110px';
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
                        getEmployeeWithFilter(false);
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-office="props">
            <q-th :props="props">
              <div :style="`width: ${widthOfOffice};`">
                <q-select
                  @update:model-value="getEmployeeWithFilter(false)"
                  dense
                  standout
                  dark
                  clearable
                  v-model="filter.officeId"
                  :options="tempListOffice"
                  :label="props.col.label"
                  option-value="id"
                  option-label="name"
                  emit-value
                  map-options
                  options-selected-class="text-accent"
                  @filter="filterOffice"
                  input-debounce="100"
                  fill-input
                  hide-selected
                  use-input
                  :label-color="labelColorFocus[1]"
                  @focus="
                    labelColorFocus[1] = 'black';
                    widthOfOffice = '140px';
                  "
                  @blur="
                    labelColorFocus[1] = 'white';
                    widthOfOffice = !filter.officeId ? '100px' : '140px';
                  "
                  @clear="getEmployeeWithFilter(false)"
                  ><template v-slot:no-option>
                    <q-item>
                      <q-item-section class="text-grey">
                        No results
                      </q-item-section>
                    </q-item>
                  </template>
                </q-select>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-fullName="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthOfFullName};`">
                <q-input
                  @update:model-value="getEmployeeWithFilter(false)"
                  dark
                  dense
                  standout
                  debounce="400"
                  v-model="filter.firstName"
                  input-class="text-right"
                  :label="labelNameFocus[2]"
                  :label-color="labelColorFocus[2]"
                  @focus="
                    labelColorFocus[2] = 'black';
                    labelNameFocus[2] = 'Search by first name';
                    widthOfFullName = '170px';
                  "
                  @blur="
                    labelColorFocus[2] = 'white';
                    labelNameFocus[2] = props.col.label;
                    widthOfFullName = '130px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filter.firstName" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filter.firstName = '';
                        getEmployeeWithFilter(false);
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-available="props">
            <q-th :props="props">
              <div>
                <q-checkbox
                  left-label
                  dense
                  dark
                  v-model="filter.available"
                  color="accent"
                  @update:model-value="getEmployeeWithFilter(false)"
                  label="Available"
                />
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-skill="props">
            <q-th :props="props">
              <div :style="`width: ${widthOfSkill};`">
                <q-select
                  dense
                  standout
                  dark
                  max-values="3"
                  clearable
                  multiple
                  use-chips
                  use-input
                  input-debounce="200"
                  v-model="filter.technologyId"
                  :options="tempListSkill"
                  :label="labelNameFocus[3]"
                  option-value="id"
                  option-label="name"
                  emit-value
                  map-options
                  options-selected-class="text-accent"
                  :label-color="labelColorFocus[3]"
                  @update:model-value="getEmployeeWithFilter(false)"
                  @filter="filterSkill"
                  @focus="
                    labelColorFocus[3] = 'black';
                    labelNameFocus[3] = 'Search by skill';
                    widthOfSkill = '250px';
                  "
                  @blur="
                    labelColorFocus[3] = 'white';
                    labelNameFocus[3] = props.col.label;
                    widthOfSkill = !filter?.technologyId?.length ? '150px' : '250px';
                  "
                  ><template v-slot:no-option>
                    <q-item>
                      <q-item-section class="text-grey">
                        No results
                      </q-item-section>
                    </q-item>
                  </template>
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
              <div>
                <q-btn
                  style="width: 60px"
                  dense
                  color="white"
                  text-color="black"
                  label="Edit"
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

          <template v-slot:body-cell-available="props">
            <q-td :props="props">
              <q-icon
                v-show="props.value"
                name="fas fa-check"
                color="positive"
                size="16px"
              />
            </q-td>
          </template>

          <template v-slot:body-cell-fullName="props">
            <q-td :props="props">
              {{ showName(props.value) }}
            </q-td>
          </template>

          <template v-slot:body-cell-skill="props">
            <q-td :props="props">
              <div v-if="props.value?.length">
                <div v-for="(item, index) in props.value" :key="index">
                  <q-badge :color="index % 2 == 0 ? 'blue-10' : 'teal-8'">
                    <span style="font-size: 14px"
                      >{{ props?.value[index].categoryName }}:</span
                    >
                  </q-badge>
                  <span>
                    {{
                      props?.value[index]?.technologies[0]?.name
                        ? ` ${props?.value[index]?.technologies[0]?.name},`
                        : ""
                    }}{{
                      props?.value[index]?.technologies[1]?.name
                        ? ` ${props?.value[index]?.technologies[1]?.name},`
                        : ""
                    }}{{
                      props?.value[index]?.technologies[2]?.name
                        ? ` ${props?.value[index]?.technologies[2]?.name},`
                        : ""
                    }}...
                  </span>
                </div>
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
  name: "List Employee",

  data() {
    return {
      labelColorFocus: ["white", "white", "white", "white"],
      labelNameFocus: ['Staff ID', '', 'Full Name', 'Skills'],

      widthOfStaffId: "110px",
      widthOfOffice: "100px",
      widthOfFullName: "130px",
      widthOfSkill: "150px",

      loadingData: false,

      showDelete: false,
      idDelete: null,
      deleteProcess: false,

      filter: {
        staffId: null,
        officeId: null,
        firstName: null,
        available: false,
        technologyId: [],
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

      tempListOffice: [],
      listOffice: [],
      tempListSkill: [],
      listSkill: [],
      listEmployee: [],
      headerTable: [
        {
          name: "index",
          label: "#",
          align: "center",
          field: "index",
        },
        {
          name: "staffId",
          align: "left",
          label: "Staff ID",
          field: "staffId",
        },
        {
          name: "office",
          align: "left",
          label: "Office",
          field: (row) => row.office.name,
        },
        {
          name: "avatar",
          align: "center",
          label: "",
          field: (row) => row.avatar.thumbnail,
        },
        {
          name: "fullName",
          align: "left",
          label: "Full Name",
          field: (row) => `${row.firstName} ${row.lastName}`,
        },
        {
          name: "available",
          align: "center",
          label: "Available",
          field: (row) => row.available,
        },
        {
          name: "skill",
          align: "left",
          label: "Skills",
          field: (row) => row.categoryPerson,
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
    ...mapActions("auth", ["useRefreshToken", "validateToken"]),

    async getEmployee() {
      try {
        this.loadingData = true;

        // Request API
        let result = await api
          .get(`/api/v1/person?page=1&pageSize=10`)
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
    async getEmployeeWithFilter(props) {
      try {
        this.loadingData = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        let { page, rowsPerPage } = props
          ? props.pagination
          : { page: 1, rowsPerPage: this.pagination.rowsPerPage };

        // Request API
        let result = await api
          .post(
            `/api/v1/person/pagination?page=${page}&pageSize=${rowsPerPage}`,
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
    async getOffice() {
      // Request API
      let result = await api
        .get("/api/v1/office")
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
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async findTechnology(keyword, seedValue) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(
          `/api/v1/technology/search?filterName=${
            !keyword ? '' : keyword.trim()
          }`
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
        this.tempListSkill = result.resource;
        if (seedValue) this.listSkill = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    mappingPagination(resource) {
      this.listEmployee = resource.resource;

      this.listEmployee.forEach((row, index) => {
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
      if (text.length > 20) return `${texttext.slice(0, 20)}...`;
      else return text;
    },
    filterOffice(val, update, abort) {
      update(() => {
        if (!val) {
          this.tempListOffice = this.listOffice.slice(0, 5);
        } else {
          let needle = val.toLowerCase();
          this.tempListOffice = this.listOffice.filter(
            (v) => v.name.toLowerCase().indexOf(needle) > -1
          );
        }
      });
    },
    filterSkill(val, update, abort) {
      update(async () => {
        if (val.length < 2) this.tempListSkill = this.listSkill;
        else await this.findTechnology(val, false);
      });
    },
    async deleteEmployee() {
      try {
        this.deleteProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .delete(`/api/v1/person/${this.idDelete}`)
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
          await this.getEmployeeWithFilter(false);

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
  },
  computed: {
    getNameDelete() {
      let tempEmployee = this.listEmployee.filter((x) => x.id == this.idDelete);

      let name = tempEmployee[0]?.firstName;

      return name ? this.showName(name) : "";
    },
  },
  async created() {
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([
      this.getEmployee(),
      this.getOffice(),
      this.findTechnology(false, true),
    ]);
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
