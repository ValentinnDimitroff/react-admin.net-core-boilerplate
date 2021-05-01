import React from 'react'
import { Edit } from 'react-admin'
import UserTitle from './common/UserTitle'
import UsersForm from './UsersForm'

const UsersEdit = (props) => {
    return (
        <Edit {...props} title={<UserTitle />}>
            <UsersForm />
        </Edit>
    )
}

export default UsersEdit
