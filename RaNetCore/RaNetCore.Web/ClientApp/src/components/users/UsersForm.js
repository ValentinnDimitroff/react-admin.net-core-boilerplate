import React from 'react'
import { SimpleForm, TextInput,  } from '../_design'
import { userRolesChoices } from '../../constants'
// SelectArrayInput
const UsersForm = (props) => {
    return (
        <SimpleForm {...props} redirect="list">
            <TextInput source="firstName" />
            <TextInput source="lastName" />
            <TextInput source="email" />
            {/* <SelectArrayInput source="roles" choices={userRolesChoices} /> */}
        </SimpleForm>
    )
}

UsersForm.propTypes = {}

export default UsersForm
