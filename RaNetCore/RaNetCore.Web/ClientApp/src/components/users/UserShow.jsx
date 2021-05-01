import React from 'react'
import { Redirect, useLocation } from 'react-router-dom'
import { ChipFieldArray, Show, ShowSplitter, SimpleShowLayout, Tab, TabbedShowLayout, TextField } from '../_design'
import UserProfileAvatar from './common/UserProfileAvatar'
import UserTitle from './common/UserTitle'

const UserShow = (props) => {
    const location = useLocation()

    if (location.pathname.endsWith('show')) {
        return <Redirect to={`${location.pathname}/activities`} />
    }

    return (
        <Show {...props} component="div" title={<UserTitle />}>
            <ShowSplitter
                leftSide={
                    <SimpleShowLayout>
                        <UserProfileAvatar />
                        <TextField source="fullName" />
                        <TextField source="email" />
                        <ChipFieldArray source="roles" />
                    </SimpleShowLayout>
                }
                rightSide={
                    <TabbedShowLayout>
                        <Tab label="Activities" path="activity">
                            <div>Empty</div>
                        </Tab>
                        <Tab label="Daily Costs" path="stats">
                            <div>Empty</div>
                        </Tab>
                    </TabbedShowLayout>
                }
            />
        </Show>
    )
}

export default UserShow
