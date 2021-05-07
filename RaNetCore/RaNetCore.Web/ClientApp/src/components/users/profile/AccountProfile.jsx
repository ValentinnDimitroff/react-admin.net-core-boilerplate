import React, { useEffect, useState } from 'react'
import Box from '@material-ui/core/Box'
import CircularProgress from '@material-ui/core/CircularProgress'
import Grid from '@material-ui/core/Grid'
import { useGetAccountProfile } from '../hooks'
import { AccountSummary, AccountDetails, AccountSecurity } from './sections'

const AccountProfile = () => {
    const { data, loading, loaded, error } = useGetAccountProfile()
    const [profileData, setProfileData] = useState()

    useEffect(() => {
        if (loaded) {
            setProfileData(data)
        }
    }, [data, loaded])

    if (error) return <div>{error}</div>

    if (loading || !profileData)
        return (
            <Box display="flex" mt={10} justifyContent="center">
                <CircularProgress />
            </Box>
        )

    return (
        <Box mt="20px">
            <Grid container spacing={4}>
                <Grid item lg={4} md={6} xl={4} xs={12}>
                    <AccountSummary profileData={profileData} />
                </Grid>
                <Grid item lg={8} md={6} xl={8} xs={12}>
                    <AccountDetails profileData={profileData} setProfileData={setProfileData} />
                    <AccountSecurity />
                </Grid>
            </Grid>
        </Box>
    )
}

export default AccountProfile
