import React from 'react'
import { Create } from '../_design'
import { SimpleForm, TextInput } from 'react-admin'

const UsersCreate = (props) => {
    return (
        <Create {...props}>
            <SimpleForm {...props} redirect="list">
                <TextInput source="firstName" />
                <TextInput source="lastName" />
                <TextInput source="email" />
            </SimpleForm>
        </Create>
    )
}

UsersCreate.propTypes = {}

export default UsersCreate
