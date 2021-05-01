import React from 'react'
import { Create, SimpleForm, TextInput  } from '../_design'

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

export default UsersCreate
