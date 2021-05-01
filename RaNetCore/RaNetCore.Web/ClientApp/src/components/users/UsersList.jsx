import React from 'react'
import {
    List,
    Datagrid,
    //TextField,
    EmailField,
    EditButton,
    DeleteButton,
    // ChipFieldArray,
    // CompactList,
    // FullNameField,
} from '../_design'

const UsersList = (props) => {
    return (
        <List
            {...props}
            // responsiveView={
            //     <CompactList
            //         primaryItems={[
            //             <TextField source="fullName" />,
            //             <ChipFieldArray source="roles" />,
            //         ]}
            //         secondaryItems={[<TextField source="email" />]}
            //         actions={[<EditButton />, <DeleteButton />]}
            //     />
            // }
        >
            <Datagrid rowClick="show">
                {/* <FullNameField source="fullName" label="Name" /> */}
                <EmailField source="email" />
                {/* <ChipFieldArray source="roles" /> */}
                <EditButton />
                <DeleteButton />
            </Datagrid>
        </List>
    )
}

export default UsersList
