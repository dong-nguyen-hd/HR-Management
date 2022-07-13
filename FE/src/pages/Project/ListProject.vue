<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container full-height full-width">
      <q-dialog v-model="showDelete" :persistent="deleteProcess">
        <q-card>
          <q-card-section class="row items-center">
            <span class="text-h6">Delete "{{ getNameDelete }}"?</span>
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
              @click="deleteGroup"
              :loading="deleteProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <q-dialog v-model="show" :persistent="true">
        <q-card style="width: 400px">
          <q-card-section class="row items-center">
            <span v-show="!showEdit" class="text-h6">Add Project</span>
            <span v-show="showEdit" class="text-h6">Edit Project</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <div class="q-mt-sm">
              <q-input
                ref="oneRef"
                standout
                clearable
                maxlength="250"
                v-model="groupResource.name"
                type="text"
                label="Name:"
                :label-color="colorFocusGroup[0]"
                @focus="colorFocusGroup[0] = 'white'"
                @blur="colorFocusGroup[0] = ''"
                :rules="[(val) => !!val || 'Name is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="twoRef"
                standout
                clearable
                autogrow
                maxlength="250"
                v-model="groupResource.description"
                type="text"
                label="Description:"
                :label-color="colorFocusGroup[1]"
                @focus="colorFocusGroup[1] = 'white'"
                @blur="colorFocusGroup[1] = ''"
                :rules="[(val) => !!val || 'Description is required']"
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
                v-model="groupResource.teamSize"
                type="number"
                label="Team Size:"
                :label-color="colorFocusGroup[2]"
                @focus="colorFocusGroup[2] = 'white'"
                @blur="colorFocusGroup[2] = ''"
                :rules="[(val) => !!val || 'Team Size is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="fourRef"
                standout
                maxlength="500"
                v-model="groupResource.startDate"
                type="text"
                placeholder="YYYY-MM-DD"
                mask="####-##-##"
                stack-label
                label="Start Date:"
                :label-color="colorFocusGroup[3]"
                @focus="colorFocusGroup[3] = 'white'"
                @blur="colorFocusGroup[3] = ''"
                :rules="[
                  (val) => !!val || 'Start Date is required',
                  (val) => validateDate(val) || 'Start Date is invalid',
                ]"
                lazy-rules="ondemand"
                hide-bottom-space
              >
                <template v-slot:append>
                  <q-icon name="event" class="cursor-pointer">
                    <q-popup-proxy
                      cover
                      transition-show="scale"
                      transition-hide="scale"
                    >
                      <q-date
                        v-model="groupResource.startDate"
                        mask="YYYY-MM-DD"
                      >
                        <div class="row items-center justify-end">
                          <q-btn
                            v-close-popup
                            label="Close"
                            color="primary"
                            flat
                          />
                        </div>
                      </q-date>
                    </q-popup-proxy>
                  </q-icon>
                </template>
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="fiveRef"
                standout
                maxlength="500"
                v-model="groupResource.endDate"
                type="text"
                placeholder="YYYY-MM-DD"
                mask="####-##-##"
                stack-label
                label="End Date:"
                :label-color="colorFocusGroup[4]"
                @focus="colorFocusGroup[4] = 'white'"
                @blur="colorFocusGroup[4] = ''"
                :rules="[(val) => validateDate(val) || 'End Date is invalid']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
                <template v-slot:append>
                  <q-icon name="event" class="cursor-pointer">
                    <q-popup-proxy
                      cover
                      transition-show="scale"
                      transition-hide="scale"
                    >
                      <q-date v-model="groupResource.endDate" mask="YYYY-MM-DD">
                        <div class="row items-center justify-end">
                          <q-btn
                            v-close-popup
                            label="Close"
                            color="primary"
                            flat
                          />
                        </div>
                      </q-date>
                    </q-popup-proxy>
                  </q-icon>
                </template>
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-select
                ref="sixRef"
                standout
                clearable
                multiple
                use-chips
                max-values="̀50"
                use-input
                v-model="groupResource.technologies"
                :options="listTechnology"
                :label="labelNameFocus[1]"
                option-value="id"
                option-label="name"
                emit-value
                map-options
                options-selected-class="text-accent"
                :label-color="colorFocusGroup[5]"
                :rules="[(val) => val?.length || 'Skill is required']"
                @filter="filterTechnology"
                @focus="
                  colorFocusGroup[5] = 'white';
                  labelNameFocus[1] = 'Search by skill';
                "
                @blur="
                  colorFocusGroup[5] = '';
                  labelNameFocus[1] = 'Skills:';
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
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="groupProcess"
              label="Cancel"
              color="primary"
              v-close-popup
            />
            <q-btn
              v-show="!showEdit"
              flat
              label="Add"
              color="info"
              @click="saveInsert"
              :loading="groupProcess"
            />
            <q-btn
              v-show="showEdit"
              flat
              label="Edit"
              color="info"
              @click="saveUpdate"
              :loading="groupProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <q-dialog v-model="showListEmployee" persistent full-width>
        <q-layout view="hHh lpR fFf" container class="bg-white">
          <q-header class="bg-accent">
            <q-toolbar class="q-pl-md">
              <q-toolbar-title
                >List employee of "{{ groupObj.name }}" project</q-toolbar-title
              >
              <q-btn flat round dense icon="close" v-close-popup />
            </q-toolbar>
          </q-header>

          <q-page-container>
            <q-page class="q-pa-md relative-position">
              <q-table
                table-style="height: 500px"
                :rows="listEmployee"
                :columns="headerTableView"
                :loading="loadingEmployee"
                :rows-per-page-options="[10, 15, 20]"
                row-key="id"
                dark
                color="amber"
              >
                <template v-slot:body-cell-avatar="props">
                  <q-td :props="props">
                    <q-avatar>
                      <img :src="props.value" />
                    </q-avatar>
                  </q-td>
                </template>

                <template v-slot:body-cell-action="props">
                  <q-td :props="props">
                    <div class="q-mr-sm" style="display: inline">
                      <q-btn
                        style="width: 60px"
                        dense
                        color="info"
                        text-color="white"
                        label="View"
                        @click="openViewDialog(props.value)"
                      />
                    </div>
                    <div class="q-mr-sm" style="display: inline">
                      <q-btn
                        style="width: 60px"
                        dense
                        color="white"
                        text-color="black"
                        label="Edit"
                        @click="openEditEmployee(props.value)"
                      />
                    </div>
                  </q-td>
                </template>

                <template v-slot:loading>
                  <q-inner-loading showing color="primary" />
                </template>
              </q-table>
            </q-page>
          </q-page-container>

          <q-footer class="bg-accent text-white">
            <q-toolbar class="flex flex-center">
              <q-btn
                v-show="false"
                dense
                color="primary"
                label="Save"
                class="q-px-lg"
              />
            </q-toolbar>
          </q-footer>
        </q-layout>
      </q-dialog>

      <q-dialog v-model="showView" full-height>
        <q-layout view="hHh lpR fFf" container class="bg-white">
          <q-header class="bg-accent">
            <q-toolbar>
              <q-toolbar-title></q-toolbar-title>
              <q-btn v-close-popup flat round dense icon="close" />
            </q-toolbar>
          </q-header>

          <q-page-container>
            <q-page>
              <q-scroll-area style="height: 581px">
                <div>
                  <div
                    class="row q-mt-sm"
                    style="height: 86px; font-size: 10px"
                  >
                    <div class="col-4">
                      <q-img
                        src="../../assets/images/logo-hybrid-technologies.svg"
                        fit="cover"
                      />
                    </div>
                    <div class="col-8">
                      <div>
                        Human Resources Department - Hybrid Technologies Co.,
                        Ltd.
                      </div>
                      <div>
                        Address: 9th Floor, Viet A Tower, Duy Tan Str., Cau Giay
                        Dist., Hanoi., Viet Nam 19F, Ichigo Shinkawa Building,
                        2-22-1 Shinkawa, Chuo-ku, Tokyo 104-0033 Japan
                      </div>
                      <div>
                        Website:
                        <span class="text-cyan-7"
                          >https://hybrid-technologies.co.jp/</span
                        >
                      </div>
                    </div>
                  </div>

                  <div class="row">
                    <div class="col-9">
                      <div
                        class="q-pl-md bg-cyan-3 flex items-center height-view text-uppercase"
                      >
                        <span class="text-bold"
                          >{{ viewObj.firstName }} {{ viewObj.lastName }}</span
                        >
                      </div>

                      <div class="row height-view-sub">
                        <div
                          class="col-4 q-pl-md flex items-center border-right"
                        >
                          <span class="text-bold">Gender</span>
                        </div>
                        <div class="col-8 q-pl-md flex items-center">
                          {{ converGender(viewObj.gender) }}
                        </div>
                      </div>

                      <div class="row height-view-sub">
                        <div
                          class="col-4 q-pl-md flex items-center border-right text-bold"
                        >
                          Date of Birth
                        </div>
                        <div class="col-8 q-pl-md flex items-center">
                          {{ convertDateTimeToDate(viewObj.dateOfBirth) }}
                        </div>
                      </div>
                    </div>

                    <div class="col-3 border-image-view">
                      <q-img :src="viewObj.avatar.original" fit="cover" />
                    </div>
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                  >
                    PROFESSIONAL OVERVIEW
                  </div>
                  <div class="q-pa-md flex items-center height-view-sub">
                    {{ viewObj.description }}
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                    v-show="viewObj.orderIndex.includes(1)"
                  >
                    SKILLS
                  </div>

                  <div
                    class="row height-view-sub"
                    v-show="viewObj.orderIndex.includes(1)"
                    v-for="(skill, index) in viewObj.categoryPerson"
                    :key="index"
                  >
                    <div
                      class="col-4 flex items-center q-pl-md border-right text-bold"
                    >
                      {{ skill.categoryName }}
                    </div>
                    <div class="col-8 flex items-center q-pl-md">
                      <span>
                        <span
                          v-for="(technology, i) in skill.technologies"
                          :key="i"
                        >
                          {{ technology.name
                          }}<span v-show="i != skill.technologies.length - 1"
                            >,
                          </span>
                        </span>
                      </span>
                    </div>
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                    v-show="viewObj.orderIndex.includes(3)"
                  >
                    WORKING HISTORY
                  </div>

                  <div
                    class="row height-view-sub"
                    v-show="viewObj.orderIndex.includes(3)"
                  >
                    <div
                      class="col-1 q-pl-md flex items-center border-right text-bold"
                    >
                      NO.
                    </div>
                    <div
                      class="col q-pl-md flex items-center border-right text-bold"
                    >
                      PREIOD
                    </div>
                    <div
                      class="col q-pl-md flex items-center border-right text-bold"
                    >
                      COMPANY
                    </div>
                    <div class="col q-pl-md flex items-center text-bold">
                      JOB TITTLE
                    </div>
                  </div>

                  <div
                    class="row height-view-sub"
                    v-show="viewObj.orderIndex.includes(3)"
                    v-for="(workHistory, index) in viewObj.workHistory"
                    :key="index"
                  >
                    <div class="col-1 q-pl-md flex items-center border-right">
                      {{ index + 1 }}
                    </div>
                    <div class="col q-pl-md flex items-center border-right">
                      <span
                        >{{
                          convertDateTimeToDate(
                            workHistory.startDate,
                            "MMM YYYY"
                          )
                        }}
                        -
                        {{
                          workHistory.endDate
                            ? convertDateTimeToDate(
                                workHistory.endDate,
                                "MMM YYYY"
                              )
                            : "Now"
                        }}</span
                      >
                    </div>
                    <div class="col q-pl-md flex items-center border-right">
                      {{ workHistory.companyName }}
                    </div>
                    <div class="col q-pl-md flex items-center">
                      {{ workHistory.position }}
                    </div>
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                    v-show="viewObj.orderIndex.includes(4)"
                  >
                    EDUCATION
                  </div>

                  <div
                    class="education-view column q-pa-md"
                    v-show="viewObj.orderIndex.includes(4)"
                    v-for="(education, index) in viewObj.education"
                    :key="index"
                  >
                    <div class="flex items-center col">
                      <span class="text-bold"
                        >{{
                          convertDateTimeToDate(education.startDate, "YYYY")
                        }}
                        -
                        {{
                          education.endDate
                            ? convertDateTimeToDate(
                                education.endDate,
                                "MMM YYYY"
                              )
                            : "Now"
                        }}
                        | {{ education.collegeName }}</span
                      >
                    </div>
                    <div class="flex items-center col">
                      <span
                        ><span class="text-bold">Major: </span
                        >{{ education.major }}</span
                      >
                    </div>
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                    v-show="viewObj.orderIndex.includes(5)"
                  >
                    CERTIFICATION
                  </div>

                  <div
                    class="height-view-sub q-pl-md flex items-center"
                    v-show="viewObj.orderIndex.includes(5)"
                    v-for="(certificate, index) in viewObj.certificate"
                    :key="index"
                  >
                    <span class="text-bold"
                      >{{
                        convertDateTimeToDate(certificate.startDate, "YYYY")
                      }}
                      | {{ certificate.name }}</span
                    >
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                    v-show="viewObj.orderIndex.includes(2)"
                  >
                    PROJECTS
                  </div>

                  <div
                    class="row height-view-sub"
                    v-show="viewObj.orderIndex.includes(2)"
                  >
                    <div
                      class="col-1 flex items-center q-pl-md border-right text-bold"
                    >
                      NO.
                    </div>
                    <div
                      class="col flex items-center q-pl-md border-right text-bold"
                    >
                      PREIOD
                    </div>
                    <div
                      class="col flex items-center q-pl-md border-right text-bold"
                    >
                      POSITION
                    </div>
                    <div class="col-5 flex items-center q-pl-md text-bold">
                      DESCRIPTION
                    </div>
                  </div>

                  <div
                    class="row project-view"
                    v-show="viewObj.orderIndex.includes(2)"
                    v-for="(project, index) in viewObj.project"
                    :key="index"
                  >
                    <div class="col-1 q-pl-md flex items-center border-right">
                      {{ index + 1 }}
                    </div>
                    <div class="col q-pl-md flex items-center border-right">
                      {{ convertDateTimeToDate(project.startDate, "MMM YYYY") }}
                      -
                      {{
                        project.endDate
                          ? convertDateTimeToDate(project.endDate, "MMM YYYY")
                          : "Now"
                      }}
                    </div>
                    <div class="col q-pl-md flex items-center border-right">
                      {{ project.position }}
                    </div>
                    <div class="col-5">
                      <div
                        class="q-pl-md project-bottom-view flex items-center project-height-view"
                      >
                        <span class="text-bold">{{ project.name }}</span>
                      </div>
                      <div
                        class="q-pl-md project-bottom-view flex items-center project-height-view"
                      >
                        <span
                          ><span class="text-bold">Description: </span
                          >{{ project.description }}</span
                        >
                      </div>
                      <div
                        class="q-pl-md project-bottom-view flex items-center project-height-view"
                      >
                        <span
                          ><span class="text-bold">Responsibilities: </span
                          >{{ project.responsibilities }}</span
                        >
                      </div>
                      <div
                        class="q-pl-md project-bottom-view flex items-center project-height-view"
                      >
                        <span
                          ><span class="text-bold">Team size: </span
                          >{{ project.teamSize }}</span
                        >
                      </div>
                      <div
                        class="q-pl-md flex items-center project-height-view"
                      >
                        <span
                          ><span class="text-bold">Technologies used: </span
                          ><span
                            v-for="(technology, i) in project.technologies"
                            :key="i"
                          >
                            {{ technology.name
                            }}<span
                              v-show="i != project.technologies.length - 1"
                              >,
                            </span>
                          </span></span
                        >
                      </div>
                    </div>
                  </div>
                </div>
              </q-scroll-area>
            </q-page>
          </q-page-container>

          <q-footer class="bg-accent text-white">
            <q-toolbar class="flex flex-center">
              <q-btn
                v-show="false"
                dense
                color="primary"
                label="Download"
                class="q-px-lg"
              />
            </q-toolbar>
          </q-footer>
        </q-layout>
      </q-dialog>

      <q-dialog v-model="showEditEmployee" persistent full-width full-height>
        <q-layout view="hHh lpR fFf" container class="bg-white">
          <q-header class="bg-accent">
            <q-toolbar>
              <q-toolbar-title></q-toolbar-title>
              <q-btn flat round dense icon="close" @click="closeEditEmployeeDialog" />
            </q-toolbar>
          </q-header>

          <q-page-container>
            <q-page>
              <edit-employee
                v-if="showEditEmployee"
                @updateSuccess="updateReceiveEmployee"
                :statusUpdateTransfer="statusUpdateEmployee"
                :employeeTransfer="editEmployeeObj"
              />
            </q-page>
          </q-page-container>

          <q-footer class="bg-accent text-white">
            <q-toolbar class="flex flex-center">
              <q-btn
                :loading="statusUpdateEmployee"
                dense
                color="primary"
                label="Save"
                class="q-px-lg"
                @click="saveUpdateEmployee"
              />
            </q-toolbar>
          </q-footer>
        </q-layout>
      </q-dialog>

      <div
        class="table-component full-height full-width flex flex-center q-px-md"
      >
        <div class="new-item q-mb-md flex justify-end full-width">
          <div class="q-mr-md" v-show="!filter.available">
            <q-input
              dense
              readonly
              clearable
              standout
              v-model="filter.lastDay"
              type="text"
              placeholder="YYYY-MM-DD"
              stack-label
              label="Day finish project:"
              label-color="white"
              bg-color="primary"
              input-class="text-white"
              hide-bottom-space
            >
              <template v-slot:append>
                <q-icon name="event" class="cursor-pointer" color="white">
                  <q-popup-proxy
                    cover
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-date
                      v-model="filter.lastDay"
                      mask="YYYY-MM-DD"
                      @update:model-value="getGroupWithFilter(false)"
                    >
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          @click="clearLastDay"
                          label="Clear"
                          color="primary"
                          flat
                        />

                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-date>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
          </div>
          <q-btn
            @click="openInsert"
            color="primary"
            label="New Project"
            unelevated
          />
        </div>

        <q-table
          class="table-content"
          :rows="listGroup"
          :columns="headerTable"
          row-key="id"
          flat
          bordered
          dark
          :loading="loadingData"
          v-model:pagination="pagination"
          :rows-per-page-options="[5, 10, 15, 20]"
          @request="getGroupWithFilter"
        >
          <template v-slot:header-cell-name="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthOfName};`">
                <q-input
                  @update:model-value="getGroupWithFilter(false)"
                  dark
                  dense
                  standout
                  debounce="400"
                  v-model="filter.name"
                  input-class="text-right"
                  :label="labelNameFocus[0]"
                  :label-color="labelColorFocus[0]"
                  @focus="
                    labelColorFocus[0] = 'black';
                    labelNameFocus[0] = 'Search by name';
                    widthOfName = '170px';
                  "
                  @blur="
                    labelColorFocus[0] = 'white';
                    labelNameFocus[0] = props.col.label;
                    widthOfName = '100px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filter.name" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filter.name = '';
                        getGroupWithFilter(false);
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
                  @update:model-value="getGroupWithFilter(false)"
                  label="Continuous"
                />
              </div>
            </q-th>
          </template>

          <template v-slot:body-cell-action="props">
            <q-td :props="props">
              <div class="q-mr-sm" style="display: inline">
                <q-btn
                  style="width: 60px"
                  dense
                  color="info"
                  text-color="white"
                  label="View"
                  @click="openView(props.value)"
                />
              </div>
              <div class="q-mr-sm" style="display: inline">
                <q-btn
                  style="width: 60px"
                  dense
                  color="white"
                  text-color="black"
                  label="Edit"
                  @click="openEdit(props.value)"
                />
              </div>
              <div style="display: inline">
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
                v-show="!props.value"
                name="fas fa-check"
                color="positive"
                size="16px"
              />
            </q-td>
          </template>

          <template v-slot:body-cell-name="props">
            <q-td class="name-cell" :props="props">
              <div>{{ reduceText(props.value) }}</div>
            </q-td>
          </template>

          <template v-slot:body-cell-description="props">
            <q-td class="description-cell" :props="props">
              <div>{{ reduceText(props.value) }}</div>
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
import { useQuasar, date } from "quasar";
import { api } from "src/boot/axios";
import EditEmployee from "src/pages/Employee/EditEmployee.vue";

export default defineComponent({
  name: "List Project",

  components: {
    "edit-employee": EditEmployee,
  },

  data() {
    return {
      labelColorFocus: ["white"],
      labelNameFocus: ["Name", "Skills:"],
      widthOfName: "100px",

      colorFocusGroup: [],

      loadingData: false,

      showDelete: false,
      idDelete: null,
      deleteProcess: false,

      show: false,
      showEdit: false,
      editObj: null,
      groupProcess: false,

      showListEmployee: false,
      groupObj: null,
      listEmployee: [],
      loadingEmployee: false,

      showView: false,
      viewObj: null,

      showEditEmployee: false,
      editEmployeeObj: null,
      statusUpdateEmployee: false,

      groupResource: {
        name: "",
        description: "",
        teamSize: null,
        startDate: null,
        endDate: null,
        technologies: [],
      },

      filter: {
        name: null,
        available: false,
        lastDay: null,
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

      listGroup: [],
      listTechnology: [],
      tempListTechnoly: [],
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
          field: "name",
        },
        {
          name: "description",
          align: "left",
          label: "Description",
          field: "description",
        },
        {
          name: "teamSize",
          align: "right",
          label: "Team Size",
          field: "teamSize",
        },
        {
          name: "available",
          align: "center",
          label: "Continuous",
          field: "isFinished",
        },
        {
          name: "action",
          align: "center",
          label: "Actions",
          field: (row) => row.id,
        },
      ],
      headerTableView: [
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
          name: "position",
          align: "left",
          label: "Position",
          field: (row) => row.position.name,
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

    openInsert() {
      this.groupResource.name = "";
      this.groupResource.description = "";
      this.groupResource.teamSize = null;
      this.groupResource.startDate = null;
      this.groupResource.endDate = "";
      this.groupResource.technologies = [];

      this.showEdit = false;
      this.show = true;
    },
    async saveInsert() {
      if (
        !this.$refs.oneRef.validate() ||
        !this.$refs.twoRef.validate() ||
        !this.$refs.threeRef.validate() ||
        !this.$refs.fourRef.validate() ||
        !this.$refs.sixRef.validate()
      ) {
        return null;
      }

      if (this.groupResource.endDate) {
        if (!this.$refs.fiveRef.validate()) return null;
      } else {
        this.groupResource.endDate = null;
      }

      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      this.groupProcess = true;

      // Request API
      let result = await api
        .post(`/api/v1/group`, this.groupResource)
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
        if (this.listGroup.length >= 10) this.listGroup.pop();
        this.listGroup.unshift(result.resource);
        this.listGroup.forEach((row, index) => {
          row.index = index + 1;
        });

        this.show = false;

        this.$q.notify({
          type: "positive",
          message: "Successfully added",
        });
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.groupProcess = false;
    },
    openEdit(id) {
      // Map obj
      this.editObj = this.listGroup.find((x) => x.id == id);
      this.groupResource.name = this.editObj.name;
      this.groupResource.description = this.editObj.description;
      this.groupResource.teamSize = this.editObj.teamSize;
      this.groupResource.startDate = this.editObj.startDate;
      this.groupResource.endDate = this.editObj.endDate;
      this.groupResource.technologies = this.editObj.technologies?.map(
        (x) => x.id
      );
      // Map list technology
      this.listTechnology = this.editObj.technologies;
      // Pop up
      this.showEdit = true;
      this.show = true;
    },
    async saveUpdate() {
      if (
        !this.$refs.oneRef.validate() ||
        !this.$refs.twoRef.validate() ||
        !this.$refs.threeRef.validate() ||
        !this.$refs.fourRef.validate() ||
        !this.$refs.sixRef.validate()
      ) {
        return null;
      }

      if (this.groupResource.endDate) {
        if (!this.$refs.fiveRef.validate()) return null;
      } else {
        this.groupResource.endDate = null;
      }

      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      this.groupProcess = true;

      // Request API
      let result = await api
        .put(`/api/v1/group/${this.editObj.id}`, this.groupResource)
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
        let index = this.listGroup.findIndex((x) => x.id == this.editObj.id);
        let rowNumber = this.listGroup[index].index;
        this.listGroup[index] = result.resource;
        this.listGroup[index].index = rowNumber;

        this.show = false;

        this.$q.notify({
          type: "positive",
          message: "Successfully updated",
        });
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.groupProcess = false;
    },
    async filterTechnology(val, update, abort) {
      update(async () => {
        if (val.length < 2) this.listTechnology = this.tempListTechnoly;
        if (val.length >= 2) await this.findTechnology(val, false);
      });
    },
    async findTechnology(keyword, seedValue) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(
          `/api/v1/technology/search?filterName=${
            !keyword ? "" : keyword.trim()
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
        if (seedValue) this.tempListTechnoly = result.resource;
        this.listTechnology = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getGroupWithFilter(props) {
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
            `/api/v1/group/pagination?page=${page}&pageSize=${rowsPerPage}`,
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
      this.listGroup = resource.resource;

      this.listGroup.forEach((row, index) => {
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
    reduceText(text) {
      if (!text) return "";

      if (text.length > 200) return `${text.slice(0, 200)}...`;
      else return text;
    },
    async deleteGroup() {
      try {
        this.deleteProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .delete(`/api/v1/group/${this.idDelete}`)
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
          await this.getGroupWithFilter(false);

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
    validateDate(dateTarget) {
      return date.isValid(dateTarget);
    },
    async clearLastDay() {
      this.filter.lastDay = null;

      await this.getGroupWithFilter(false);
    },
    async openView(id) {
      try {
        this.groupObj = this.listGroup.find((x) => x.id == id);
        this.showListEmployee = true;
        this.loadingEmployee = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .get(`/api/v1/group/list-person-edit/${id}`)
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
          this.listEmployee = [...result.resource];

          this.listEmployee.forEach((row, index) => {
            row.index = index + 1;
          });
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.loadingEmployee = false;
      }
    },
    convertDateTimeToDate(dateTime, stringFormat = "YYYY-MM-DD") {
      return date.formatDate(dateTime, stringFormat);
    },
    openViewDialog(id) {
      this.viewObj = this.listEmployee.find((x) => x.id == id);
      this.showView = true;
    },
    converGender(value) {
      if (value == 1) return "Male";
      if (value == 2) return "Female";
      if (value == 3) return "Sexless";
    },
    openEditEmployee(id) {
      this.editEmployeeObj = this.listEmployee.find((x) => x.id == id);
      this.showEditEmployee = true;
    },
    saveUpdateEmployee() {
      this.statusUpdateEmployee = true;
    },
    async updateReceiveEmployee(value) {
      if (value) {
        let result = await this.getEmployeeById(this.editEmployeeObj.id);

        this.statusUpdateEmployee = false;
        this.showEditEmployee = false;

        let index = this.listEmployee.findIndex((x) => x.id == this.editEmployeeObj.id);
        let numberIndex = this.listEmployee[index].index;
        this.listEmployee[index] = Object.assign({}, result);
        this.listEmployee[index].index = numberIndex;
        this.editEmployeeObj = null;
      } else {
        this.statusUpdateEmployee = false;
      }
    },
    async closeEditEmployeeDialog() {
      let result = await this.getEmployeeById(this.editEmployeeObj.id);

      let index = this.listEmployee.findIndex((x) => x.id == this.editEmployeeObj.id);
      let numberIndex = this.listEmployee[index].index;
      this.listEmployee[index] = Object.assign({}, result);
      this.listEmployee[index].index = numberIndex;
      this.editEmployeeObj = null;

      this.statusUpdateEmployee = false;
      this.showEditEmployee = false;
    },
    async getEmployeeById(id) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(`/api/v1/person/${id}`)
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

      return result?.resource;
    },
  },
  computed: {
    getNameDelete() {
      let tempEmployee = this.listGroup.filter((x) => x.id == this.idDelete);

      let name = tempEmployee[0]?.name;

      return name ? this.reduceText(name) : "";
    },
  },
  async created() {
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([
      this.getGroupWithFilter(0),
      this.findTechnology("", true),
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

.name-cell {
  max-width: 200px;
  white-space: initial;
}

.description-cell {
  max-width: 400px;
  white-space: initial;
}

// view component
.height-view {
  min-height: 52px;
  border: 1px solid $primary;
  border-bottom: 0;
}

.height-view-sub {
  min-height: 44px;
  border: 1px solid $primary;
  border-bottom: 0;
}

.border-right {
  border-right: 1px solid $primary;
}

.education-view {
  min-height: 71px;
  border: 1px solid $primary;
  border-bottom: 0;
}

.border-image-view {
  border-top: 1px solid $primary;
  border-right: 1px solid $primary;
}

.project-view {
  border: 1px solid $primary;
  border-bottom: 0;
}

.project-bottom-view {
  border-bottom: 1px solid $primary;
}

.project-height-view {
  min-height: 30px;
}
// view component
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
