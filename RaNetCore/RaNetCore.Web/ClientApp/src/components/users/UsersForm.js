import React from 'react'
import { SimpleForm, SelectArrayInput, TextInput } from '../_design'
import { userRolesChoices } from '../../constants'

const UsersForm = (props) => {
    return (
        <SimpleForm {...props} redirect="list">
            <TextInput source="firstName" />
            <TextInput source="lastName" />
            <TextInput source="email" />
            <SelectArrayInput source="roles" choices={userRolesChoices} />
        </SimpleForm>
    )
}

export default UsersForm
