<template>
  <q-page class="fit">
    <div class="row container">
      <div class="col-6 left-side flex flex-center">
        <div class="fit q-py-lg">
          <q-scroll-area style="height: 532px">
            <div class="avatar flex flex-center column">
              <q-avatar size="100px">
                <img :src="imageURL" />
              </q-avatar>
              <q-file
                class="q-mt-sm cursor-pointer"
                dense
                tabindex="1"
                clearable
                standout
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

            <div class="first-name q-mt-lg q-px-lg">
              <q-input
                ref="firstNameRef"
                tabindex="2"
                standout
                clearable
                maxlength="500"
                v-model="employeeInfor.firstName"
                type="text"
                label="First Name:"
                :label-color="labelColorFocus[0]"
                @focus="labelColorFocus[0] = 'white'"
                @blur="labelColorFocus[0] = ''"
                :rules="[(val) => !!val || 'First Name is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="last-name q-mt-sm q-px-lg">
              <q-input
                ref="lastNameRef"
                tabindex="3"
                standout
                clearable
                maxlength="500"
                v-model="employeeInfor.lastName"
                type="text"
                label="Last Name:"
                :label-color="labelColorFocus[1]"
                @focus="labelColorFocus[1] = 'white'"
                @blur="labelColorFocus[1] = ''"
                :rules="[(val) => !!val || 'Last Name is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              ></q-input>
            </div>

            <div class="dob q-mt-sm q-px-lg">
              <q-input
                ref="dobRef"
                tabindex="4"
                standout
                maxlength="500"
                v-model="employeeInfor.dateOfBirth"
                type="text"
                placeholder="YYYY-MM-DD"
                mask="####-##-##"
                stack-label
                label="Date Of Birth:"
                :label-color="labelColorFocus[2]"
                @focus="labelColorFocus[2] = 'white'"
                @blur="labelColorFocus[2] = ''"
                :rules="[
                  (val) => !!val || 'DoB is required',
                  (val) => validateDoB(val) || 'DoB is invalid',
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
                        v-model="employeeInfor.dateOfBirth"
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

            <div class="gender q-mt-sm q-px-lg">
              <q-select
                standout
                ref="genderRef"
                tabindex="5"
                v-model="employeeInfor.gender"
                :options="listGender"
                label="Gender:"
                option-value="id"
                option-label="name"
                emit-value
                map-options
                options-selected-class="text-accent"
                :rules="[(val) => !!val || 'Gender is required']"
                lazy-rules="ondemand"
                hide-bottom-space
                :label-color="labelColorFocus[3]"
                @focus="labelColorFocus[3] = 'white'"
                @blur="labelColorFocus[3] = ''"
              >
              </q-select>
            </div>

            <div class="description q-mt-sm q-px-lg">
              <q-input
                ref="descriptionRef"
                standout
                tabindex="6"
                clearable
                maxlength="500"
                v-model="employeeInfor.description"
                type="text"
                autogrow
                label="Description:"
                :label-color="labelColorFocus[4]"
                @focus="labelColorFocus[4] = 'white'"
                @blur="labelColorFocus[4] = ''"
                hide-bottom-space
              ></q-input>
            </div>

            <div class="position q-mt-sm q-px-lg">
              <q-select
                standout
                ref="positionRef"
                tabindex="7"
                v-model="employeeInfor.positionId"
                :options="tempListPosition"
                label="Position:"
                option-value="id"
                option-label="name"
                emit-value
                map-options
                options-selected-class="text-accent"
                @filter="filterPosition"
                fill-input
                hide-selected
                use-input
                :rules="[(val) => !!val || 'Position is required']"
                lazy-rules="ondemand"
                hide-bottom-space
                :label-color="labelColorFocus[5]"
                @focus="labelColorFocus[5] = 'white'"
                @blur="labelColorFocus[5] = ''"
                ><template v-slot:no-option>
                  <q-item>
                    <q-item-section class="text-grey">
                      No results
                    </q-item-section>
                  </q-item>
                </template>
              </q-select>
            </div>

            <div class="email q-mt-sm q-px-lg">
              <q-input
                ref="emailRef"
                tabindex="8"
                standout
                clearable
                maxlength="500"
                v-model="employeeInfor.email"
                type="email"
                label="Email:"
                :label-color="labelColorFocus[6]"
                @focus="labelColorFocus[6] = 'white'"
                @blur="labelColorFocus[6] = ''"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="phone q-mt-sm q-px-lg">
              <q-input
                ref="phoneRef"
                mask="#########################"
                standout
                clearable
                tabindex="9"
                maxlength="25"
                v-model="employeeInfor.phone"
                type="text"
                label="Phone:"
                :label-color="labelColorFocus[7]"
                @focus="labelColorFocus[7] = 'white'"
                @blur="labelColorFocus[7] = ''"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="work q-mt-sm q-px-lg">
              <q-select
                standout
                ref="workRef"
                tabindex="10"
                clearable
                v-model="employeeInfor.groupId"
                :options="tempListWork"
                label="Current Project:"
                option-value="id"
                option-label="name"
                emit-value
                map-options
                options-selected-class="text-accent"
                @filter="filterWork"
                fill-input
                hide-selected
                use-input
                hide-bottom-space
                :label-color="labelColorFocus[8]"
                @focus="labelColorFocus[8] = 'white'"
                @blur="labelColorFocus[8] = ''"
                ><template v-slot:no-option>
                  <q-item>
                    <q-item-section class="text-grey">
                      No results
                    </q-item-section>
                  </q-item>
                </template>
              </q-select>
            </div>
          </q-scroll-area>
        </div>
      </div>
      <div class="col-6 right-side">
        <div class="fit q-pa-lg">
          <q-card>
            <q-tabs
              v-model="tab"
              dense
              outside-arrows
              mobile-arrows
              class="text-grey"
              active-color="primary"
              indicator-color="primary"
              align="justify"
              narrow-indicator
            >
              <q-tab
                v-for="item in tabModel"
                :key="item.id"
                :name="item.id"
                :label="item.name"
              />
            </q-tabs>

            <q-separator />

            <q-tab-panels v-model="tab" animated>
              <q-tab-panel name="1" class="tab-skill flex flex-center">
                <div class="full-width relative-position flex flex-center">
                  <q-btn-group rounded flat unelevated>
                    <q-btn
                      color="primary"
                      label="<"
                      @click="orderTabPrev()"
                      size="md"
                    />
                    <q-btn
                      :color="booleanTab(1) ? 'primary' : 'grey'"
                      label="Skill"
                      @click="toggleTab(1)"
                      size="md"
                    >
                      <q-tooltip anchor="top middle" self="center middle">
                        {{ booleanTab(1) ? "Hide" : "Show" }}
                      </q-tooltip></q-btn
                    >
                    <q-btn
                      color="primary"
                      label=">"
                      @click="orderTabNext()"
                      size="md"
                    />
                  </q-btn-group>

                  <q-btn
                    size="12px"
                    class="absolute-top-right"
                    icon="add"
                    color="primary"
                    round
                    unelevated
                    @click="openDialog(1)"
                  />
                </div>

                <div class="fit">
                  <q-scroll-area class="q-mt-md" style="height: 411px">
                    <q-card
                      v-for="(category, index) in listTempCategory"
                      :key="index"
                      bordered
                      class="inside"
                    >
                      <q-card-section horizontal class="row">
                        <q-card-section class="col-10">
                          <div class="q-mb-sm">
                            <q-badge
                              class="text-subtitle2"
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            >
                              <span>{{ category?.name }}:</span>
                            </q-badge>
                          </div>

                          <span
                            v-for="skill in category?.technologies"
                            :key="skill?.id"
                            class="q-mr-sm"
                          >
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                            >
                              {{ skill?.name }}
                            </q-badge>
                          </span>
                        </q-card-section>

                        <q-card-actions vertical class="col-2 justify-around">
                          <q-btn
                            @click="orderSkillPrev(category)"
                            flat
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_up"
                          />

                          <q-fab
                            class="q-my-sm"
                            icon="more_horiz"
                            direction="left"
                            padding="sm"
                            unelevated
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                          >
                            <q-fab-action
                              @click="editSkill(category)"
                              color="info"
                              icon="edit"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Edit
                              </q-tooltip></q-fab-action
                            >
                            <q-fab-action
                              @click="deleteSkill(category)"
                              color="negative"
                              icon="delete"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Delete
                              </q-tooltip></q-fab-action
                            >
                          </q-fab>

                          <q-btn
                            flat
                            @click="orderSkillNext(category)"
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_down"
                          />
                        </q-card-actions>
                      </q-card-section>
                    </q-card>
                  </q-scroll-area>
                </div>

                <q-dialog v-model="dialogToggle[1]" persistent>
                  <q-card style="width: 400px">
                    <q-card-section class="row items-center">
                      <span class="text-h6">{{
                        showEditBtn ? "Edit Skill" : "Add Skill"
                      }}</span>
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <div class="q-mt-sm">
                        <q-select
                          standout
                          ref="categoryRef"
                          v-model="tempCategoryPersonResource.categoryId"
                          :options="tempListCategory"
                          :label="labelNameFocusSkill[0]"
                          :option-disable="
                            (item) =>
                              getDisableCategory.some((x) => x == item.id)
                                ? true
                                : false
                          "
                          option-value="id"
                          option-label="name"
                          emit-value
                          map-options
                          options-selected-class="text-accent"
                          @filter="filterCategory"
                          fill-input
                          input-debounce="200"
                          hide-selected
                          use-input
                          :rules="[(val) => !!val || 'Position is required']"
                          hide-bottom-space
                          :label-color="colorFocusSkill[0]"
                          @focus="
                            colorFocusSkill[0] = 'white';
                            labelNameFocusSkill[0] = 'Search by category';
                          "
                          @blur="
                            colorFocusSkill[0] = '';
                            labelNameFocusSkill[0] = 'Category:';
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

                      <div class="q-mt-sm">
                        <q-select
                          ref="skillRef"
                          standout
                          clearable
                          multiple
                          use-chips
                          max-values="̀50"
                          use-input
                          :disable="
                            tempCategoryPersonResource.categoryId ? false : true
                          "
                          v-model="tempCategoryPersonResource.technologies"
                          :options="tempListSkillCategory"
                          :label="labelNameFocusSkill[1]"
                          option-value="id"
                          option-label="name"
                          emit-value
                          map-options
                          options-selected-class="text-accent"
                          :label-color="colorFocusSkill[1]"
                          :rules="[(val) => val?.length || 'Skill is required']"
                          @filter="filterSkillBelongWithCategory"
                          @focus="
                            colorFocusSkill[1] = 'white';
                            labelNameFocusSkill[1] = 'Search by skill';
                          "
                          @blur="
                            colorFocusSkill[1] = '';
                            labelNameFocusSkill[1] = 'Skill:';
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
                        label="Cancel"
                        color="primary"
                        v-close-popup
                      />
                      <q-btn
                        flat
                        label="Add"
                        color="info"
                        @click="addSkill"
                        v-show="!showEditBtn"
                      />
                      <q-btn
                        flat
                        label="Save"
                        color="info"
                        @click="saveSkill"
                        v-show="showEditBtn"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>

                <q-dialog v-model="deleteToggle[1]" persistent>
                  <q-card>
                    <q-card-section class="row items-center">
                      <span class="text-h6"
                        >Delete {{ tempDeleteSkill.name }} category?</span
                      >
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <span>This can’t be undone and it will be removed.</span>
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        v-close-popup
                        label="Cancel"
                        color="primary"
                      />
                      <q-btn
                        flat
                        label="Delete"
                        color="negative"
                        @click="saveDeleteSkill"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>
              </q-tab-panel>

              <q-tab-panel name="2" class="tab-project flex flex-center">
                <div class="full-width relative-position flex flex-center">
                  <q-btn-group rounded flat unelevated>
                    <q-btn
                      color="primary"
                      label="<"
                      @click="orderTabPrev()"
                      size="md"
                    />
                    <q-btn
                      :color="booleanTab(2) ? 'primary' : 'grey'"
                      label="Project"
                      @click="toggleTab(2)"
                      size="md"
                    >
                      <q-tooltip anchor="top middle" self="center middle">
                        {{ booleanTab(2) ? "Hide" : "Show" }}
                      </q-tooltip></q-btn
                    >
                    <q-btn
                      color="primary"
                      label=">"
                      @click="orderTabNext()"
                      size="md"
                    />
                  </q-btn-group>

                  <q-btn
                    size="12px"
                    class="absolute-top-right"
                    icon="add"
                    color="primary"
                    round
                    unelevated
                    @click="openDialog(2)"
                  />
                </div>

                <div class="fit">
                  <q-scroll-area class="q-mt-md" style="height: 411px">
                    <q-card
                      v-for="(project, index) in listTempProject"
                      :key="index"
                      bordered
                      class="inside"
                    >
                      <q-card-section horizontal class="row">
                        <q-card-section class="col-10">
                          <div class="q-mb-sm">
                            <q-badge
                              class="text-subtitle2"
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            >
                              <span>{{ project?.groupName }}:</span>
                            </q-badge>
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Position:</q-badge
                            >
                            {{ project.position }}
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Responsibilities:</q-badge
                            >
                            {{ project.responsibilities }}
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Start Date:</q-badge
                            >
                            {{ project.startDate }}
                          </div>

                          <div v-show="project.endDate">
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >End Date:</q-badge
                            >
                            {{ project.endDate }}
                          </div>
                        </q-card-section>

                        <q-card-actions vertical class="col-2 justify-around">
                          <q-btn
                            @click="orderProjectPrev(project)"
                            flat
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_up"
                          />

                          <q-fab
                            class="q-my-sm"
                            icon="more_horiz"
                            direction="left"
                            padding="sm"
                            unelevated
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                          >
                            <q-fab-action
                              @click="editProject(project)"
                              color="info"
                              icon="edit"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Edit
                              </q-tooltip></q-fab-action
                            >
                            <q-fab-action
                              @click="deleteProject(project)"
                              color="negative"
                              icon="delete"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Delete
                              </q-tooltip></q-fab-action
                            >
                          </q-fab>

                          <q-btn
                            flat
                            @click="orderProjectNext(project)"
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_down"
                          />
                        </q-card-actions>
                      </q-card-section>
                    </q-card>
                  </q-scroll-area>
                </div>

                <q-dialog v-model="dialogToggle[2]" persistent>
                  <q-card style="width: 400px">
                    <q-card-section class="row items-center">
                      <span class="text-h6">{{
                        showEditBtn ? "Edit Project" : "Add Project"
                      }}</span>
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <div class="q-mt-sm">
                        <q-select
                          standout
                          ref="projectOneRef"
                          v-model="tempProjectResource.groupId"
                          :options="tempListGroup"
                          :label="labelNameFocusProject[0]"
                          option-value="id"
                          option-label="name"
                          emit-value
                          map-options
                          options-selected-class="text-accent"
                          @filter="filterProject"
                          fill-input
                          input-debounce="200"
                          hide-selected
                          use-input
                          :rules="[(val) => !!val || 'Project is required']"
                          hide-bottom-space
                          :label-color="colorFocusProject[0]"
                          @focus="
                            colorFocusProject[0] = 'white';
                            labelNameFocusProject[0] = 'Search by project';
                          "
                          @blur="
                            colorFocusProject[0] = '';
                            labelNameFocusProject[0] = 'Project:';
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

                      <div class="q-mt-sm">
                        <q-input
                          ref="projectTwoRef"
                          standout
                          clearable
                          maxlength="500"
                          v-model="tempProjectResource.position"
                          type="text"
                          label="Position:"
                          :label-color="colorFocusProject[1]"
                          @focus="colorFocusProject[1] = 'white'"
                          @blur="colorFocusProject[1] = ''"
                          hide-bottom-space
                          :rules="[(val) => !!val || 'Position is required']"
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="projectThreeRef"
                          standout
                          clearable
                          maxlength="500"
                          autogrow
                          v-model="tempProjectResource.responsibilities"
                          type="text"
                          label="Responsibilities:"
                          :label-color="colorFocusProject[2]"
                          @focus="colorFocusProject[2] = 'white'"
                          @blur="colorFocusProject[2] = ''"
                          :rules="[
                            (val) => !!val || 'Responsibilities is required',
                          ]"
                          hide-bottom-space
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="projectFourRef"
                          standout
                          maxlength="500"
                          v-model="tempProjectResource.startDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="Start Date:"
                          :label-color="colorFocusProject[3]"
                          @focus="colorFocusProject[3] = 'white'"
                          @blur="colorFocusProject[3] = ''"
                          :rules="[
                            (val) => !!val || 'Start Date is required',
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
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
                                  v-model="tempProjectResource.startDate"
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
                          ref="projectFiveRef"
                          standout
                          maxlength="500"
                          v-model="tempProjectResource.endDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="End Date:"
                          :label-color="colorFocusProject[4]"
                          @focus="colorFocusProject[4] = 'white'"
                          @blur="colorFocusProject[4] = ''"
                          :rules="[
                            (val) => !!val || 'End Date is required',
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
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
                                  v-model="tempProjectResource.endDate"
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
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        label="Cancel"
                        color="primary"
                        v-close-popup
                      />
                      <q-btn
                        flat
                        label="Add"
                        color="info"
                        @click="addProject"
                        v-show="!showEditBtn"
                      />
                      <q-btn
                        flat
                        label="Save"
                        color="info"
                        @click="saveProject"
                        v-show="showEditBtn"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>

                <q-dialog v-model="deleteToggle[2]" persistent>
                  <q-card>
                    <q-card-section class="row items-center">
                      <span class="text-h6"
                        >Delete {{ tempDeleteProject.groupName }} project?</span
                      >
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <span>This can’t be undone and it will be removed.</span>
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        v-close-popup
                        label="Cancel"
                        color="primary"
                      />
                      <q-btn
                        flat
                        label="Delete"
                        color="negative"
                        @click="saveDeleteProject"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>
              </q-tab-panel>

              <q-tab-panel name="3" class="tab-project flex flex-center">
                <div class="full-width relative-position flex flex-center">
                  <q-btn-group rounded flat unelevated>
                    <q-btn
                      color="primary"
                      label="<"
                      @click="orderTabPrev()"
                      size="md"
                    />
                    <q-btn
                      :color="booleanTab(3) ? 'primary' : 'grey'"
                      label="Work History"
                      @click="toggleTab(3)"
                      size="md"
                    >
                      <q-tooltip anchor="top middle" self="center middle">
                        {{ booleanTab(3) ? "Hide" : "Show" }}
                      </q-tooltip></q-btn
                    >
                    <q-btn
                      color="primary"
                      label=">"
                      @click="orderTabNext()"
                      size="md"
                    />
                  </q-btn-group>

                  <q-btn
                    size="12px"
                    class="absolute-top-right"
                    icon="add"
                    color="primary"
                    round
                    unelevated
                    @click="openDialog(3)"
                  />
                </div>

                <div class="fit">
                  <q-scroll-area class="q-mt-md" style="height: 411px">
                    <q-card
                      v-for="(
                        workHistory, index
                      ) in employeeInfor.workHistoryResource"
                      :key="index"
                      bordered
                      class="inside"
                    >
                      <q-card-section horizontal class="row">
                        <q-card-section class="col-10">
                          <div class="q-mb-sm">
                            <q-badge
                              class="text-subtitle2"
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            >
                              <span>{{ workHistory.companyName }}:</span>
                            </q-badge>
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Position:</q-badge
                            >
                            {{ workHistory.position }}
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Start Date:</q-badge
                            >
                            {{ workHistory.startDate }}
                          </div>

                          <div v-show="workHistory.endDate">
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >End Date:</q-badge
                            >
                            {{ workHistory.endDate }}
                          </div>
                        </q-card-section>

                        <q-card-actions vertical class="col-2 justify-around">
                          <q-btn
                            @click="orderWorkHistoryPrev(workHistory)"
                            flat
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_up"
                          />

                          <q-fab
                            class="q-my-sm"
                            icon="more_horiz"
                            direction="left"
                            padding="sm"
                            unelevated
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                          >
                            <q-fab-action
                              @click="editWorkHistory(workHistory)"
                              color="info"
                              icon="edit"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Edit
                              </q-tooltip></q-fab-action
                            >
                            <q-fab-action
                              @click="deleteWorkHistory(workHistory)"
                              color="negative"
                              icon="delete"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Delete
                              </q-tooltip></q-fab-action
                            >
                          </q-fab>

                          <q-btn
                            flat
                            @click="orderWorkHistoryNext(workHistory)"
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_down"
                          />
                        </q-card-actions>
                      </q-card-section>
                    </q-card>
                  </q-scroll-area>
                </div>

                <q-dialog v-model="dialogToggle[3]" persistent>
                  <q-card style="width: 400px">
                    <q-card-section class="row items-center">
                      <span class="text-h6">{{
                        showEditBtn ? "Edit Work History" : "Add History"
                      }}</span>
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <div class="q-mt-sm">
                        <q-input
                          ref="workHistoryOneRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempWorkHistoryResource.companyName"
                          type="text"
                          label="Company Name:"
                          :label-color="colorFocusWorkHistory[0]"
                          @focus="colorFocusWorkHistory[0] = 'white'"
                          @blur="colorFocusWorkHistory[0] = ''"
                          :rules="[
                            (val) => !!val || 'Company Name is required',
                          ]"
                          hide-bottom-space
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="workHistoryTwoRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempWorkHistoryResource.position"
                          type="text"
                          label="Position:"
                          :label-color="colorFocusWorkHistory[1]"
                          @focus="colorFocusWorkHistory[1] = 'white'"
                          @blur="colorFocusWorkHistory[1] = ''"
                          hide-bottom-space
                          :rules="[(val) => !!val || 'Position is required']"
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="workHistoryThreeRef"
                          standout
                          maxlength="250"
                          v-model="tempWorkHistoryResource.startDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="Start Date:"
                          :label-color="colorFocusWorkHistory[2]"
                          @focus="colorFocusWorkHistory[2] = 'white'"
                          @blur="colorFocusWorkHistory[2] = ''"
                          :rules="[
                            (val) => !!val || 'Start Date is required',
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
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
                                  v-model="tempWorkHistoryResource.startDate"
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
                          ref="workHistoryFourRef"
                          standout
                          maxlength="250"
                          v-model="tempWorkHistoryResource.endDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="End Date:"
                          :label-color="colorFocusWorkHistory[3]"
                          @focus="colorFocusWorkHistory[3] = 'white'"
                          @blur="colorFocusWorkHistory[3] = ''"
                          hide-bottom-space
                          :rules="[
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
                        >
                          <template v-slot:append>
                            <q-icon name="event" class="cursor-pointer">
                              <q-popup-proxy
                                cover
                                transition-show="scale"
                                transition-hide="scale"
                              >
                                <q-date
                                  v-model="tempWorkHistoryResource.endDate"
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
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        label="Cancel"
                        color="primary"
                        v-close-popup
                      />
                      <q-btn
                        flat
                        label="Add"
                        color="info"
                        @click="addWorkHistory"
                        v-show="!showEditBtn"
                      />
                      <q-btn
                        flat
                        label="Save"
                        color="info"
                        @click="saveWorkHistory"
                        v-show="showEditBtn"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>

                <q-dialog v-model="deleteToggle[3]" persistent>
                  <q-card>
                    <q-card-section class="row items-center">
                      <span class="text-h6"
                        >Delete {{ tempDeleteWorkHistory.companyName }} work
                        history?</span
                      >
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <span>This can’t be undone and it will be removed.</span>
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        v-close-popup
                        label="Cancel"
                        color="primary"
                      />
                      <q-btn
                        flat
                        label="Delete"
                        color="negative"
                        @click="saveDeleteWorkHistory"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>
              </q-tab-panel>

              <q-tab-panel name="4" class="tab-project flex flex-center">
                <div class="full-width relative-position flex flex-center">
                  <q-btn-group rounded flat unelevated>
                    <q-btn
                      color="primary"
                      label="<"
                      @click="orderTabPrev()"
                      size="md"
                    />
                    <q-btn
                      :color="booleanTab(4) ? 'primary' : 'grey'"
                      label="Education"
                      @click="toggleTab(4)"
                      size="md"
                    >
                      <q-tooltip anchor="top middle" self="center middle">
                        {{ booleanTab(4) ? "Hide" : "Show" }}
                      </q-tooltip></q-btn
                    >
                    <q-btn
                      color="primary"
                      label=">"
                      @click="orderTabNext()"
                      size="md"
                    />
                  </q-btn-group>

                  <q-btn
                    size="12px"
                    class="absolute-top-right"
                    icon="add"
                    color="primary"
                    round
                    unelevated
                    @click="openDialog(4)"
                  />
                </div>

                <div class="fit">
                  <q-scroll-area class="q-mt-md" style="height: 411px">
                    <q-card
                      v-for="(
                        education, index
                      ) in employeeInfor.educationResource"
                      :key="index"
                      bordered
                      class="inside"
                    >
                      <q-card-section horizontal class="row">
                        <q-card-section class="col-10">
                          <div class="q-mb-sm">
                            <q-badge
                              class="text-subtitle2"
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            >
                              <span>{{ education.collegeName }}:</span>
                            </q-badge>
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Position:</q-badge
                            >
                            {{ education.major }}
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Start Date:</q-badge
                            >
                            {{ education.startDate }}
                          </div>

                          <div v-show="education.endDate">
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >End Date:</q-badge
                            >
                            {{ education.endDate }}
                          </div>
                        </q-card-section>

                        <q-card-actions vertical class="col-2 justify-around">
                          <q-btn
                            @click="orderEducationPrev(education)"
                            flat
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_up"
                          />

                          <q-fab
                            class="q-my-sm"
                            icon="more_horiz"
                            direction="left"
                            padding="sm"
                            unelevated
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                          >
                            <q-fab-action
                              @click="editEducation(education)"
                              color="info"
                              icon="edit"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Edit
                              </q-tooltip></q-fab-action
                            >
                            <q-fab-action
                              @click="deleteEducation(education)"
                              color="negative"
                              icon="delete"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Delete
                              </q-tooltip></q-fab-action
                            >
                          </q-fab>

                          <q-btn
                            flat
                            @click="orderEducationNext(education)"
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_down"
                          />
                        </q-card-actions>
                      </q-card-section>
                    </q-card>
                  </q-scroll-area>
                </div>

                <q-dialog v-model="dialogToggle[4]" persistent>
                  <q-card style="width: 400px">
                    <q-card-section class="row items-center">
                      <span class="text-h6">{{
                        showEditBtn ? "Edit Education" : "Add Education"
                      }}</span>
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <div class="q-mt-sm">
                        <q-input
                          ref="educationOneRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempEducationResource.collegeName"
                          type="text"
                          label="College Name:"
                          :label-color="colorFocusEducation[0]"
                          @focus="colorFocusEducation[0] = 'white'"
                          @blur="colorFocusEducation[0] = ''"
                          :rules="[
                            (val) => !!val || 'College Name is required',
                          ]"
                          hide-bottom-space
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="educationTwoRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempEducationResource.major"
                          type="text"
                          label="Major:"
                          :label-color="colorFocusEducation[1]"
                          @focus="colorFocusEducation[1] = 'white'"
                          @blur="colorFocusEducation[1] = ''"
                          hide-bottom-space
                          :rules="[(val) => !!val || 'Major is required']"
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="educationThreeRef"
                          standout
                          maxlength="250"
                          v-model="tempEducationResource.startDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="Start Date:"
                          :label-color="colorFocusEducation[2]"
                          @focus="colorFocusEducation[2] = 'white'"
                          @blur="colorFocusEducation[2] = ''"
                          :rules="[
                            (val) => !!val || 'Start Date is required',
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
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
                                  v-model="tempEducationResource.startDate"
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
                          ref="workHistoryFourRef"
                          standout
                          maxlength="250"
                          v-model="tempEducationResource.endDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="End Date:"
                          :label-color="colorFocusEducation[3]"
                          @focus="colorFocusEducation[3] = 'white'"
                          @blur="colorFocusEducation[3] = ''"
                          hide-bottom-space
                          :rules="[
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
                        >
                          <template v-slot:append>
                            <q-icon name="event" class="cursor-pointer">
                              <q-popup-proxy
                                cover
                                transition-show="scale"
                                transition-hide="scale"
                              >
                                <q-date
                                  v-model="tempEducationResource.endDate"
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
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        label="Cancel"
                        color="primary"
                        v-close-popup
                      />
                      <q-btn
                        flat
                        label="Add"
                        color="info"
                        @click="addEducation"
                        v-show="!showEditBtn"
                      />
                      <q-btn
                        flat
                        label="Save"
                        color="info"
                        @click="saveEducation"
                        v-show="showEditBtn"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>

                <q-dialog v-model="deleteToggle[4]" persistent>
                  <q-card>
                    <q-card-section class="row items-center">
                      <span class="text-h6"
                        >Delete
                        {{ tempDeleteEducation.collegeName }} education?</span
                      >
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <span>This can’t be undone and it will be removed.</span>
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        v-close-popup
                        label="Cancel"
                        color="primary"
                      />
                      <q-btn
                        flat
                        label="Delete"
                        color="negative"
                        @click="saveDeleteEducation"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>
              </q-tab-panel>

              <q-tab-panel name="5" class="tab-project flex flex-center">
                <div class="full-width relative-position flex flex-center">
                  <q-btn-group rounded flat unelevated>
                    <q-btn
                      color="primary"
                      label="<"
                      @click="orderTabPrev()"
                      size="md"
                    />
                    <q-btn
                      :color="booleanTab(5) ? 'primary' : 'grey'"
                      label="Certificate"
                      @click="toggleTab(5)"
                      size="md"
                    >
                      <q-tooltip anchor="top middle" self="center middle">
                        {{ booleanTab(5) ? "Hide" : "Show" }}
                      </q-tooltip></q-btn
                    >
                    <q-btn
                      color="primary"
                      label=">"
                      @click="orderTabNext()"
                      size="md"
                    />
                  </q-btn-group>

                  <q-btn
                    size="12px"
                    class="absolute-top-right"
                    icon="add"
                    color="primary"
                    round
                    unelevated
                    @click="openDialog(5)"
                  />
                </div>

                <div class="fit">
                  <q-scroll-area class="q-mt-md" style="height: 411px">
                    <q-card
                      v-for="(
                        certificate, index
                      ) in employeeInfor.certificateResource"
                      :key="index"
                      bordered
                      class="inside"
                    >
                      <q-card-section horizontal class="row">
                        <q-card-section class="col-10">
                          <div class="q-mb-sm">
                            <q-badge
                              class="text-subtitle2"
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            >
                              <span>{{ certificate.name }}:</span>
                            </q-badge>
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Position:</q-badge
                            >
                            {{ certificate.provider }}
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Start Date:</q-badge
                            >
                            {{ certificate.startDate }}
                          </div>

                          <div v-show="certificate.endDate">
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >End Date:</q-badge
                            >
                            {{ certificate.endDate }}
                          </div>
                        </q-card-section>

                        <q-card-actions vertical class="col-2 justify-around">
                          <q-btn
                            @click="orderCertificatePrev(certificate)"
                            flat
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_up"
                          />

                          <q-fab
                            class="q-my-sm"
                            icon="more_horiz"
                            direction="left"
                            padding="sm"
                            unelevated
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                          >
                            <q-fab-action
                              @click="editCertificate(certificate)"
                              color="info"
                              icon="edit"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Edit
                              </q-tooltip></q-fab-action
                            >
                            <q-fab-action
                              @click="deleteCertificate(certificate)"
                              color="negative"
                              icon="delete"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Delete
                              </q-tooltip></q-fab-action
                            >
                          </q-fab>

                          <q-btn
                            flat
                            @click="orderCertificateNext(certificate)"
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_down"
                          />
                        </q-card-actions>
                      </q-card-section>
                    </q-card>
                  </q-scroll-area>
                </div>

                <q-dialog v-model="dialogToggle[5]" persistent>
                  <q-card style="width: 400px">
                    <q-card-section class="row items-center">
                      <span class="text-h6">{{
                        showEditBtn ? "Edit Certificate" : "Add Certificate"
                      }}</span>
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <div class="q-mt-sm">
                        <q-input
                          ref="certificateOneRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempCertificateResource.name"
                          type="text"
                          label="Name:"
                          :label-color="colorFocusCertificate[0]"
                          @focus="colorFocusCertificate[0] = 'white'"
                          @blur="colorFocusCertificate[0] = ''"
                          :rules="[(val) => !!val || 'Name is required']"
                          hide-bottom-space
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="certificateTwoRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempCertificateResource.provider"
                          type="text"
                          label="Provider:"
                          :label-color="colorFocusCertificate[1]"
                          @focus="colorFocusCertificate[1] = 'white'"
                          @blur="colorFocusCertificate[1] = ''"
                          hide-bottom-space
                          :rules="[(val) => !!val || 'Provider is required']"
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="certificateThreeRef"
                          standout
                          maxlength="250"
                          v-model="tempCertificateResource.startDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="Start Date:"
                          :label-color="colorFocusCertificate[2]"
                          @focus="colorFocusCertificate[2] = 'white'"
                          @blur="colorFocusCertificate[2] = ''"
                          :rules="[
                            (val) => !!val || 'Start Date is required',
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
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
                                  v-model="tempCertificateResource.startDate"
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
                          ref="workHistoryFourRef"
                          standout
                          maxlength="250"
                          v-model="tempCertificateResource.endDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="End Date:"
                          :label-color="colorFocusCertificate[3]"
                          @focus="colorFocusCertificate[3] = 'white'"
                          @blur="colorFocusCertificate[3] = ''"
                          hide-bottom-space
                          :rules="[
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
                        >
                          <template v-slot:append>
                            <q-icon name="event" class="cursor-pointer">
                              <q-popup-proxy
                                cover
                                transition-show="scale"
                                transition-hide="scale"
                              >
                                <q-date
                                  v-model="tempCertificateResource.endDate"
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
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        label="Cancel"
                        color="primary"
                        v-close-popup
                      />
                      <q-btn
                        flat
                        label="Add"
                        color="info"
                        @click="addCertificate"
                        v-show="!showEditBtn"
                      />
                      <q-btn
                        flat
                        label="Save"
                        color="info"
                        @click="saveCertificate"
                        v-show="showEditBtn"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>

                <q-dialog v-model="deleteToggle[5]" persistent>
                  <q-card>
                    <q-card-section class="row items-center">
                      <span class="text-h6"
                        >Delete
                        {{ tempDeleteCertificate.name }} certificate?</span
                      >
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <span>This can’t be undone and it will be removed.</span>
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        v-close-popup
                        label="Cancel"
                        color="primary"
                      />
                      <q-btn
                        flat
                        label="Delete"
                        color="negative"
                        @click="saveDeleteCertificate"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>
              </q-tab-panel>
            </q-tab-panels>
          </q-card>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import { mapGetters, mapActions } from "vuex";
import { useQuasar, date } from "quasar";
import { api } from "src/boot/axios";

export default defineComponent({
  name: "New Employee",

  props: {
    statusInsertTransfer: Boolean,
  },

  data() {
    return {
      tab: "1",
      tabModel: [],

      constTabModel: [
        { id: "1", name: "Skill" },
        { id: "2", name: "Project" },
        { id: "3", name: "Work History" },
        { id: "4", name: "Education" },
        { id: "5", name: "Certificate" },
      ],

      avatarDefault: "",
      imageURL: null,
      imageFile: null,

      employeeInfor: {
        firstName: "",
        lastName: "",
        email: null,
        description: "",
        phone: null,
        dateOfBirth: "",
        positionId: null,
        groupId: null,
        gender: "",
        orderIndex: [1, 2, 3, 4, 5],

        categoryPersonResource: [],
        certificateResource: [],
        educationResource: [],
        projectResource: [],
        workHistoryResource: [],
      },

      tempCategoryPersonResource: {
        personId: 0,
        categoryId: 0,
        technologies: [],
      },
      listTempCategory: [],

      tempProjectResource: {
        position: "",
        responsibilities: "",
        startDate: "",
        endDate: null,
        personId: 0,
        groupId: 0,
        tempId: null,
      },
      listTempProject: [],

      tempWorkHistoryResource: {
        companyName: "",
        position: "",
        startDate: null,
        endDate: null,
        personId: 0,
        tempId: null,
      },

      tempEducationResource: {
        collegeName: "",
        major: "",
        startDate: "",
        endDate: null,
        personId: 0,
        tempId: null,
      },

      tempCertificateResource: {
        name: "",
        provider: "",
        startDate: "",
        endDate: null,
        personId: 0,
        tempId: null,
      },

      tempDeleteSkill: null,
      tempDeleteProject: null,
      tempDeleteWorkHistory: null,
      tempDeleteEducation: null,
      tempDeleteCertificate: null,

      loadingSave: false,
      componentKey: 0,

      labelColorFocus: [],
      colorFocusSkill: [],
      colorFocusProject: [],
      colorFocusWorkHistory: [],
      colorFocusEducation: [],
      colorFocusCertificate: [],
      labelNameFocusSkill: ["Category:", "Skill:"],
      labelNameFocusProject: ["Project:"],
      showEditBtn: false,

      listGender: [
        {
          name: "Male",
          id: 1,
        },
        {
          name: "Female",
          id: 2,
        },
        {
          name: "Sexless",
          id: 3,
        },
      ],
      dialogToggle: [],
      deleteToggle: [],
      tempOrderIndex: [],
      tempListSkillCategory: [],
      tempListCategory: [],
      listCategory: [],
      tempListPosition: [],
      listPosition: [],
      tempListGroup: [],
      tempListWork: [],
      listGroup: [],
    };
  },
  methods: {
    ...mapActions("auth", ["validateToken"]),

    async findCategory(keyword, seedValue) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(
          `/api/v1/category/search?filterName=${!keyword ? "" : keyword.trim()}`
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
        this.tempListCategory = result.resource;
        if (seedValue) this.listCategory = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async findGroup(keyword, seedValue, component) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(
          `/api/v1/group/search?filterName=${!keyword ? "" : keyword.trim()}`
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
        if (component == "project") this.tempListGroup = result.resource;
        if (component == "work") this.tempListWork = result.resource;
        if (seedValue) this.listGroup = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getByCategoryId(id) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(`/api/v1/category/${id}`)
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
        let category = result.resource;
        this.tempListCategory = [];
        this.tempListCategory.unshift(category);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getByGroupId(id) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(`/api/v1/group/${id}`)
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
        return result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getPosition() {
      // Request API
      let result = await api
        .get("/api/v1/position")
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
        this.listPosition = result.resource;

        // Mode edit
        if (this.employeeInfor.positionId) {
          this.tempListPosition = this.listPosition.filter(
            (v) => v.id == this.employeeInfor.positionId
          );
        }
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getAvatarUrl() {
      // Request API
      let result = await api
        .get(`/api/v1/image/default-image`)
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
        this.avatarDefault = result?.resource?.originalImagePath;
      }
    },
    previewImage() {
      this.imageURL = this.imageFile
        ? URL.createObjectURL(this.imageFile)
        : null;
    },
    clearTempImage() {
      this.imageURL = this.avatarDefault;
    },
    onRejected(rejectedEntries) {
      this.$q.notify({
        type: "negative",
        message: `Image size must lower than 5 MB`,
      });
    },
    filterPosition(val, update, abort) {
      update(() => {
        if (!val) {
          this.tempListPosition = this.listPosition.slice(0, 5);
        } else {
          let needle = val.toLowerCase();
          this.tempListPosition = this.listPosition.filter(
            (v) => v.name.toLowerCase().indexOf(needle) > -1
          );
        }
      });
    },
    filterCategory(val, update, abort) {
      update(async () => {
        if (val.length > 0 && val.length < 2)
          this.tempListCategory = this.listCategory;
        if (val.length >= 2) await this.findCategory(val, false);
      });
    },
    filterProject(val, update, abort) {
      update(async () => {
        if (!this.tempListGroup?.length) this.tempListGroup = this.listGroup;
        if (val.length > 0 && val.length < 2)
          this.tempListGroup = this.listGroup;
        if (val.length >= 2) await this.findGroup(val, false, "project");
      });
    },
    filterWork(val, update, abort) {
      update(async () => {
        if (!this.tempListWork?.length) this.tempListWork = this.listGroup;
        if (val.length > 0 && val.length < 2)
          this.tempListWork = this.listGroup;
        if (val.length >= 2) await this.findGroup(val, false, "work");
      });
    },
    filterSkillBelongWithCategory(val, update, abort) {
      update(() => {
        let temp = this.getSkillBelongWithCategory();
        if (!val) {
          this.tempListSkillCategory = temp;
        } else {
          let needle = val.toLowerCase();
          this.tempListSkillCategory = temp.filter(
            (v) => v.name.toLowerCase().indexOf(needle) > -1
          );
        }
      });
    },
    getSkillBelongWithCategory() {
      let categoryId = this.tempCategoryPersonResource.categoryId;

      let obj = this.tempListCategory.find((x) => x.id == categoryId);

      return obj.technologies;
    },
    openDialog(index) {
      if (index == 1) {
        this.tempCategoryPersonResource.categoryId = 0;
        this.tempCategoryPersonResource.technologies = [];
      }

      if (index == 2) {
        this.tempProjectResource.position = "";
        this.tempProjectResource.responsibilities = "";
        this.tempProjectResource.startDate = "";
        this.tempProjectResource.endDate = null;
        this.tempProjectResource.groupId = 0;
      }

      if (index == 3) {
        this.tempWorkHistoryResource.companyName = "";
        this.tempWorkHistoryResource.position = "";
        this.tempWorkHistoryResource.startDate = null;
        this.tempWorkHistoryResource.endDate = null;
      }

      if (index == 4) {
        this.tempEducationResource.collegeName = "";
        this.tempEducationResource.major = "";
        this.tempEducationResource.startDate = null;
        this.tempEducationResource.endDate = null;
      }

      if (index == 5) {
        this.tempCertificateResource.name = "";
        this.tempCertificateResource.provider = "";
        this.tempCertificateResource.startDate = null;
        this.tempCertificateResource.endDate = null;
      }

      this.dialogToggle[index] = true;
    },
    orderTabNext() {
      // Print UI
      let countTab = this.tabModel.length;
      let indexTab = this.tabModel.findIndex((x) => x.id == this.tab);

      if (indexTab == countTab - 1) {
        let temp = this.tabModel.slice(0, indexTab);
        temp.unshift(this.tabModel[indexTab]);
        this.tabModel = temp;
      } else {
        for (let i = 0; i < countTab; i++) {
          if (i == indexTab) {
            let temp = this.tabModel[i];
            this.tabModel[i] = this.tabModel[i + 1];
            this.tabModel[i + 1] = temp;
            break;
          }
        }
      }

      // Array send to server
      let countModel = this.employeeInfor.orderIndex.length;
      let indexModel = this.employeeInfor.orderIndex.findIndex(
        (x) => x == parseInt(this.tab)
      );

      if (indexModel == countModel - 1) {
        let temp = this.employeeInfor.orderIndex.slice(0, indexModel);
        temp.unshift(this.employeeInfor.orderIndex[indexModel]);
        this.employeeInfor.orderIndex = temp;
      } else {
        for (let i = 0; i < countModel; i++) {
          if (i == indexModel) {
            let temp = this.employeeInfor.orderIndex[i];
            this.employeeInfor.orderIndex[i] =
              this.employeeInfor.orderIndex[i + 1];
            this.employeeInfor.orderIndex[i + 1] = temp;
            break;
          }
        }
      }
    },
    orderTabPrev() {
      // Array send to server
      let countTab = this.tabModel.length;
      let indexTab = this.tabModel.findIndex((x) => x.id == this.tab);

      if (indexTab == 0) {
        let temp = this.tabModel.slice(1);
        temp.push(this.tabModel[indexTab]);
        this.tabModel = temp;
      } else {
        for (let i = 0; i < countTab; i++) {
          if (i == indexTab) {
            let temp = this.tabModel[i];
            this.tabModel[i] = this.tabModel[i - 1];
            this.tabModel[i - 1] = temp;
            break;
          }
        }
      }

      // Array send to server
      let countModel = this.employeeInfor.orderIndex.length;
      let indexModel = this.employeeInfor.orderIndex.findIndex(
        (x) => x == parseInt(this.tab)
      );

      if (indexModel == 0) {
        let temp = this.employeeInfor.orderIndex.slice(1);
        temp.push(this.employeeInfor.orderIndex[indexModel]);
        this.employeeInfor.orderIndex = temp;
      } else {
        for (let i = 0; i < countModel; i++) {
          if (i == indexModel) {
            let temp = this.employeeInfor.orderIndex[i];
            this.employeeInfor.orderIndex[i] =
              this.employeeInfor.orderIndex[i - 1];
            this.employeeInfor.orderIndex[i - 1] = temp;
            break;
          }
        }
      }
    },
    toggleTab(val) {
      let index = this.employeeInfor.orderIndex.findIndex(
        (x) => x == parseInt(val)
      );

      let oldIndex = this.tempOrderIndex[parseInt(val)];

      if (index >= 0) {
        this.employeeInfor.orderIndex.splice(index, 1);
        this.tempOrderIndex[parseInt(val)] = index;
      } else if (oldIndex >= 0) {
        let left = this.employeeInfor.orderIndex.slice(0, oldIndex);
        let right = this.employeeInfor.orderIndex.slice(oldIndex);

        this.employeeInfor.orderIndex = left.concat(parseInt(val), right);
      } else {
        this.employeeInfor.orderIndex.push(parseInt(val));
      }
    },
    booleanTab(val) {
      return this.employeeInfor.orderIndex.some((x) => x == parseInt(val));
    },
    addSkill() {
      if (
        !this.$refs.categoryRef.validate() ||
        !this.$refs.skillRef.validate()
      ) {
        return null;
      }

      // List for send to server
      this.employeeInfor.categoryPersonResource.unshift(
        Object.assign({}, this.tempCategoryPersonResource)
      );

      // List for print
      let category = this.tempListCategory.find(
        (x) => x.id == this.tempCategoryPersonResource.categoryId
      );
      category.technologies = category.technologies.filter((x) =>
        this.tempCategoryPersonResource.technologies.includes(x.id)
      );
      this.listTempCategory.unshift(category);

      this.dialogToggle[1] = false;
    },
    async editSkill(item) {
      await this.getByCategoryId(item.id);

      let temp = [];
      item.technologies.forEach((x) => temp.push(x.id));

      this.tempCategoryPersonResource.categoryId = item.id;
      this.tempCategoryPersonResource.technologies = temp;

      this.tempListSkillCategory = this.getSkillBelongWithCategory();

      this.showEditBtn = true;
      this.dialogToggle[1] = true;
    },
    saveSkill() {
      // Set list API
      let index = this.employeeInfor.categoryPersonResource.findIndex(
        (x) => x.categoryId == this.tempCategoryPersonResource.categoryId
      );
      this.employeeInfor.categoryPersonResource[index] = Object.assign(
        {},
        this.tempCategoryPersonResource
      );

      // Mapping with print-list
      let category = this.tempListCategory.find(
        (x) => x.id == this.tempCategoryPersonResource.categoryId
      );
      category.technologies = category.technologies.filter((x) =>
        this.tempCategoryPersonResource.technologies.includes(x.id)
      );

      this.listTempCategory[index] = category;

      this.showEditBtn = false;
      this.dialogToggle[1] = false;
    },
    deleteSkill(item) {
      this.tempDeleteSkill = item;
      this.deleteToggle[1] = true;
    },
    saveDeleteSkill() {
      // Set list API
      let index = this.employeeInfor.categoryPersonResource.findIndex(
        (x) => x.categoryId == this.tempDeleteSkill.id
      );
      this.employeeInfor.categoryPersonResource.splice(index, 1);

      // Set list temp print
      this.listTempCategory.splice(index, 1);

      this.deleteToggle[1] = false;
    },
    orderSkillNext(item) {
      let count = this.employeeInfor.categoryPersonResource.length;
      // Set list API
      let index = this.employeeInfor.categoryPersonResource.findIndex(
        (x) => x.categoryId == item.id
      );

      if (index == count - 1) {
        let tempOne = this.employeeInfor.categoryPersonResource.slice(0, index);
        tempOne.unshift(this.employeeInfor.categoryPersonResource[index]);
        this.employeeInfor.categoryPersonResource = tempOne;

        let tempTwo = this.listTempCategory.slice(0, index);
        tempTwo.unshift(this.listTempCategory[index]);
        this.listTempCategory = tempTwo;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.categoryPersonResource[i];
            this.employeeInfor.categoryPersonResource[i] =
              this.employeeInfor.categoryPersonResource[i + 1];
            this.employeeInfor.categoryPersonResource[i + 1] = tempOne;

            let tempTwo = this.listTempCategory[i];
            this.listTempCategory[i] = this.listTempCategory[i + 1];
            this.listTempCategory[i + 1] = tempTwo;

            break;
          }
        }
      }
    },
    orderSkillPrev(item) {
      let count = this.employeeInfor.categoryPersonResource.length;
      // Set list API
      let index = this.employeeInfor.categoryPersonResource.findIndex(
        (x) => x.categoryId == item.id
      );

      if (index == 0) {
        let tempOne = this.employeeInfor.categoryPersonResource.slice(1);
        tempOne.push(this.employeeInfor.categoryPersonResource[index]);
        this.employeeInfor.categoryPersonResource = tempOne;

        let tempTwo = this.listTempCategory.slice(1);
        tempTwo.push(this.listTempCategory[index]);
        this.listTempCategory = tempTwo;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.categoryPersonResource[i];
            this.employeeInfor.categoryPersonResource[i] =
              this.employeeInfor.categoryPersonResource[i - 1];
            this.employeeInfor.categoryPersonResource[i - 1] = tempOne;

            let tempTwo = this.listTempCategory[i];
            this.listTempCategory[i] = this.listTempCategory[i - 1];
            this.listTempCategory[i - 1] = tempTwo;

            break;
          }
        }
      }
    },
    addProject() {
      if (
        !this.$refs.projectOneRef.validate() ||
        !this.$refs.projectTwoRef.validate() ||
        !this.$refs.projectThreeRef.validate() ||
        !this.$refs.projectFourRef.validate()
      ) {
        return null;
      }
      this.tempProjectResource.tempId = Date.now();

      // List for send to server
      this.employeeInfor.projectResource.unshift(
        Object.assign({}, this.tempProjectResource)
      );

      // List for print
      let temp = Object.assign({}, this.tempProjectResource);
      let group = this.tempListGroup.find(
        (x) => x.id == this.tempProjectResource.groupId
      );
      temp.groupName = group.name;
      this.listTempProject.unshift(temp);

      this.dialogToggle[2] = false;
    },
    saveProject() {
      // Set list API
      let index = this.employeeInfor.projectResource.findIndex(
        (x) => x.tempId == this.tempProjectResource.tempId
      );
      this.employeeInfor.projectResource[index] = Object.assign(
        {},
        this.tempProjectResource
      );

      // Mapping with print-list
      let temp = Object.assign({}, this.tempProjectResource);
      let group = this.tempListGroup.find(
        (x) => x.id == this.tempProjectResource.groupId
      );
      temp.groupName = group.name;
      this.listTempProject[index] = temp;

      this.showEditBtn = false;
      this.dialogToggle[2] = false;
    },
    async editProject(item) {
      let result = await this.getByGroupId(item.groupId);

      this.tempListGroup = [];
      this.tempListGroup.unshift(result);

      this.tempProjectResource = item;

      this.showEditBtn = true;
      this.dialogToggle[2] = true;
    },
    deleteProject(item) {
      this.tempDeleteProject = item;
      this.deleteToggle[2] = true;
    },
    saveDeleteProject() {
      // Set list API
      let index = this.employeeInfor.projectResource.findIndex(
        (x) => x.tempId == this.tempDeleteProject.tempId
      );
      this.employeeInfor.projectResource.splice(index, 1);

      // Set list temp print
      this.listTempProject.splice(index, 1);

      this.deleteToggle[2] = false;
    },
    orderProjectNext(item) {
      let count = this.employeeInfor.projectResource.length;
      // Set list API
      let index = this.employeeInfor.projectResource.findIndex(
        (x) => x.tempId == item.tempId
      );

      if (index == count - 1) {
        let tempOne = this.employeeInfor.projectResource.slice(0, index);
        tempOne.unshift(this.employeeInfor.projectResource[index]);
        this.employeeInfor.projectResource = tempOne;

        let tempTwo = this.listTempProject.slice(0, index);
        tempTwo.unshift(this.listTempProject[index]);
        this.listTempProject = tempTwo;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.projectResource[i];
            this.employeeInfor.projectResource[i] =
              this.employeeInfor.projectResource[i + 1];
            this.employeeInfor.projectResource[i + 1] = tempOne;

            let tempTwo = this.listTempProject[i];
            this.listTempProject[i] = this.listTempProject[i + 1];
            this.listTempProject[i + 1] = tempTwo;

            break;
          }
        }
      }
    },
    orderProjectPrev(item) {
      let count = this.employeeInfor.projectResource.length;
      // Set list API
      let index = this.employeeInfor.projectResource.findIndex(
        (x) => x.tempId == item.tempId
      );

      if (index == 0) {
        let tempOne = this.employeeInfor.projectResource.slice(1);
        tempOne.push(this.employeeInfor.projectResource[index]);
        this.employeeInfor.projectResource = tempOne;

        let tempTwo = this.listTempProject.slice(1);
        tempTwo.push(this.listTempProject[index]);
        this.listTempProject = tempTwo;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.projectResource[i];
            this.employeeInfor.projectResource[i] =
              this.employeeInfor.projectResource[i - 1];
            this.employeeInfor.projectResource[i - 1] = tempOne;

            let tempTwo = this.listTempProject[i];
            this.listTempProject[i] = this.listTempProject[i - 1];
            this.listTempProject[i - 1] = tempTwo;

            break;
          }
        }
      }
    },
    orderWorkHistoryNext(item) {
      let count = this.employeeInfor.workHistoryResource.length;
      // Set list API
      let index = this.employeeInfor.workHistoryResource.findIndex(
        (x) => x.tempId == item.tempId
      );

      if (index == count - 1) {
        let tempOne = this.employeeInfor.workHistoryResource.slice(0, index);
        tempOne.unshift(this.employeeInfor.workHistoryResource[index]);
        this.employeeInfor.workHistoryResource = tempOne;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.workHistoryResource[i];
            this.employeeInfor.workHistoryResource[i] =
              this.employeeInfor.workHistoryResource[i + 1];
            this.employeeInfor.workHistoryResource[i + 1] = tempOne;

            break;
          }
        }
      }
    },
    orderWorkHistoryPrev(item) {
      let count = this.employeeInfor.workHistoryResource.length;
      // Set list API
      let index = this.employeeInfor.workHistoryResource.findIndex(
        (x) => x.tempId == item.tempId
      );

      if (index == 0) {
        let tempOne = this.employeeInfor.workHistoryResource.slice(1);
        tempOne.push(this.employeeInfor.workHistoryResource[index]);
        this.employeeInfor.workHistoryResource = tempOne;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.workHistoryResource[i];
            this.employeeInfor.workHistoryResource[i] =
              this.employeeInfor.workHistoryResource[i - 1];
            this.employeeInfor.workHistoryResource[i - 1] = tempOne;

            break;
          }
        }
      }
    },
    addWorkHistory() {
      if (
        !this.$refs.workHistoryOneRef.validate() ||
        !this.$refs.workHistoryTwoRef.validate() ||
        !this.$refs.workHistoryThreeRef.validate()
      ) {
        return null;
      }
      this.tempWorkHistoryResource.tempId = Date.now();

      // List for send to server
      this.employeeInfor.workHistoryResource.unshift(
        Object.assign({}, this.tempWorkHistoryResource)
      );

      this.dialogToggle[3] = false;
    },
    saveWorkHistory() {
      // Set list API
      let index = this.employeeInfor.workHistoryResource.findIndex(
        (x) => x.tempId == this.tempWorkHistoryResource.tempId
      );

      this.employeeInfor.workHistoryResource[index] = Object.assign(
        {},
        this.tempWorkHistoryResource
      );

      this.showEditBtn = false;
      this.dialogToggle[3] = false;
    },
    editWorkHistory(item) {
      this.tempWorkHistoryResource = item;

      this.showEditBtn = true;
      this.dialogToggle[3] = true;
    },
    deleteWorkHistory(item) {
      this.tempDeleteWorkHistory = item;
      this.deleteToggle[3] = true;
    },
    saveDeleteWorkHistory() {
      // Set list API
      let index = this.employeeInfor.workHistoryResource.findIndex(
        (x) => x.tempId == this.tempDeleteWorkHistory.tempId
      );
      this.employeeInfor.workHistoryResource.splice(index, 1);

      this.deleteToggle[3] = false;
    },
    addEducation() {
      if (
        !this.$refs.educationOneRef.validate() ||
        !this.$refs.educationTwoRef.validate() ||
        !this.$refs.educationThreeRef.validate()
      ) {
        return null;
      }
      this.tempEducationResource.tempId = Date.now();

      // List for send to server
      this.employeeInfor.educationResource.unshift(
        Object.assign({}, this.tempEducationResource)
      );

      this.dialogToggle[4] = false;
    },
    saveEducation() {
      // Set list API
      let index = this.employeeInfor.educationResource.findIndex(
        (x) => x.tempId == this.tempEducationResource.tempId
      );

      this.employeeInfor.educationResource[index] = Object.assign(
        {},
        this.tempEducationResource
      );

      this.showEditBtn = false;
      this.dialogToggle[4] = false;
    },
    editEducation(item) {
      this.tempEducationResource = item;

      this.showEditBtn = true;
      this.dialogToggle[4] = true;
    },
    deleteEducation(item) {
      this.tempDeleteEducation = item;
      this.deleteToggle[4] = true;
    },
    saveDeleteEducation() {
      // Set list API
      let index = this.employeeInfor.educationResource.findIndex(
        (x) => x.tempId == this.tempDeleteEducation.tempId
      );
      this.employeeInfor.educationResource.splice(index, 1);

      this.deleteToggle[4] = false;
    },
    orderEducationNext(item) {
      let count = this.employeeInfor.educationResource.length;
      // Set list API
      let index = this.employeeInfor.educationResource.findIndex(
        (x) => x.tempId == item.tempId
      );

      if (index == count - 1) {
        let tempOne = this.employeeInfor.educationResource.slice(0, index);
        tempOne.unshift(this.employeeInfor.educationResource[index]);
        this.employeeInfor.educationResource = tempOne;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.educationResource[i];
            this.employeeInfor.educationResource[i] =
              this.employeeInfor.educationResource[i + 1];
            this.employeeInfor.educationResource[i + 1] = tempOne;

            break;
          }
        }
      }
    },
    orderEducationPrev(item) {
      let count = this.employeeInfor.educationResource.length;
      // Set list API
      let index = this.employeeInfor.educationResource.findIndex(
        (x) => x.tempId == item.tempId
      );

      if (index == 0) {
        let tempOne = this.employeeInfor.educationResource.slice(1);
        tempOne.push(this.employeeInfor.educationResource[index]);
        this.employeeInfor.educationResource = tempOne;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.educationResource[i];
            this.employeeInfor.educationResource[i] =
              this.employeeInfor.educationResource[i - 1];
            this.employeeInfor.educationResource[i - 1] = tempOne;

            break;
          }
        }
      }
    },
    addCertificate() {
      if (
        !this.$refs.certificateOneRef.validate() ||
        !this.$refs.certificateTwoRef.validate() ||
        !this.$refs.certificateThreeRef.validate()
      ) {
        return null;
      }
      this.tempCertificateResource.tempId = Date.now();

      // List for send to server
      this.employeeInfor.certificateResource.unshift(
        Object.assign({}, this.tempCertificateResource)
      );

      this.dialogToggle[5] = false;
    },
    saveCertificate() {
      // Set list API
      let index = this.employeeInfor.certificateResource.findIndex(
        (x) => x.tempId == this.tempCertificateResource.tempId
      );

      this.employeeInfor.certificateResource[index] = Object.assign(
        {},
        this.tempCertificateResource
      );

      this.showEditBtn = false;
      this.dialogToggle[5] = false;
    },
    editCertificate(item) {
      this.tempCertificateResource = item;

      this.showEditBtn = true;
      this.dialogToggle[5] = true;
    },
    deleteCertificate(item) {
      this.tempDeleteCertificate = item;
      this.deleteToggle[5] = true;
    },
    saveDeleteCertificate() {
      // Set list API
      let index = this.employeeInfor.certificateResource.findIndex(
        (x) => x.tempId == this.tempDeleteCertificate.tempId
      );
      this.employeeInfor.certificateResource.splice(index, 1);

      this.deleteToggle[5] = false;
    },
    orderCertificateNext(item) {
      let count = this.employeeInfor.certificateResource.length;
      // Set list API
      let index = this.employeeInfor.certificateResource.findIndex(
        (x) => x.tempId == item.tempId
      );

      if (index == count - 1) {
        let tempOne = this.employeeInfor.certificateResource.slice(0, index);
        tempOne.unshift(this.employeeInfor.certificateResource[index]);
        this.employeeInfor.certificateResource = tempOne;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.certificateResource[i];
            this.employeeInfor.certificateResource[i] =
              this.employeeInfor.certificateResource[i + 1];
            this.employeeInfor.certificateResource[i + 1] = tempOne;

            break;
          }
        }
      }
    },
    orderCertificatePrev(item) {
      let count = this.employeeInfor.certificateResource.length;
      // Set list API
      let index = this.employeeInfor.certificateResource.findIndex(
        (x) => x.tempId == item.tempId
      );

      if (index == 0) {
        let tempOne = this.employeeInfor.certificateResource.slice(1);
        tempOne.push(this.employeeInfor.certificateResource[index]);
        this.employeeInfor.certificateResource = tempOne;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.certificateResource[i];
            this.employeeInfor.certificateResource[i] =
              this.employeeInfor.certificateResource[i - 1];
            this.employeeInfor.certificateResource[i - 1] = tempOne;

            break;
          }
        }
      }
    },
    async requestPostEmployee(payload) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .post(`/api/v1/person`, payload)
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
        return result;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });

        return result;
      }
    },
    async requestUpdateImage(id) {
      let fd = new FormData();
      fd.append("image", this.imageFile);

      // Request API update image
      return api
        .put(`/api/v1/person/image/${id}`, fd)
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
    async saveAll() {
      try {
        if (
          !this.$refs.firstNameRef.validate() ||
          !this.$refs.lastNameRef.validate() ||
          !this.$refs.dobRef.validate() ||
          !this.$refs.genderRef.validate() ||
          !this.$refs.positionRef.validate()
        ) {
          return null;
        }

        this.loadingSave = true;

        let result = await this.requestPostEmployee(this.employeeInfor);
        if (result.success) {
          if (this.imageFile)
            await this.requestUpdateImage(result?.resource?.id);
        } else {
          this.$emit("insertSuccess", false);
          return null;
        }

        this.$q.notify({
          type: "positive",
          message: "Successfully added",
        });

        this.$emit("insertSuccess", result?.resource?.id);

        this.employeeInfor.firstName = "";
        this.employeeInfor.lastName = "";
        this.employeeInfor.email = null;
        this.employeeInfor.description = "";
        this.employeeInfor.phone = null;
        this.employeeInfor.dateOfBirth = "";
        this.employeeInfor.positionId = null;
        this.employeeInfor.groupId = null;
        this.employeeInfor.gender = "";
        this.employeeInfor.orderIndex = [1, 2, 3, 4, 5];
        this.employeeInfor.categoryPersonResource = [];
        this.employeeInfor.certificateResource = [];
        this.employeeInfor.educationResource = [];
        this.employeeInfor.projectResource = [];
        this.employeeInfor.workHistoryResource = [];
        this.tabModel = [...this.constTabModel];

        this.imageURL = this.avatarDefault;
        this.imageFile = null;

        this.listTempCategory = [];
        this.listTempProject = [];
      } finally {
        this.loadingSave = false;
      }
    },
    async resetAvatar() {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .put(`/api/v1/person/reset-avatar/${this.employeeInfor.id}`)
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
    validateDoB(dateTarget) {
      if (!date.isValid(dateTarget)) return false;

      let now = new Date(Date.now());

      let dateFrom = date.subtractFromDate(now, { years: 100 });
      let dateTo = date.addToDate(now, { years: 15 });

      return date.isBetweenDates(dateTarget, dateFrom, dateTo, {
        onlyDate: true,
      });
    },
    validateDate(dateTarget) {
      return date.isValid(dateTarget);
    },
  },
  computed: {
    ...mapGetters("auth", ["getInformation"]),
    getDisableCategory() {
      let temp = [];
      this.employeeInfor.categoryPersonResource.forEach((x) =>
        temp.push(x.categoryId)
      );

      return temp;
    },
    getCategoryId() {
      return this.tempCategoryPersonResource.categoryId;
    },
  },
  async created() {
    this.tabModel = [...this.constTabModel];

    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([
      this.findCategory(false, true),
      this.findGroup(false, true),
      this.getPosition(),
      this.getAvatarUrl(),
    ]);

    this.imageURL = this.avatarDefault;
  },
  watch: {
    getCategoryId: function (newVal, oldVal) {
      if (oldVal && newVal) this.tempCategoryPersonResource.technologies = [];
    },
    statusInsertTransfer: {
      immediate: true,
      deep: true,
      async handler(newValue, oldValue) {
        if (newValue) {
          await this.saveAll();
        }
      },
    },
  },
  mounted() {
    const $q = useQuasar();
  },
});
</script>

<style lang="scss" scoped>
.container {
  min-height: inherit;
  height: 100%;
  width: 100%;
}

.inside {
  margin: 16px;
  &:last-child {
    margin: 16px 16px 0 16px;
  }
  &:first-child {
    margin: 0 16px 16px 16px;
  }
}
</style>
