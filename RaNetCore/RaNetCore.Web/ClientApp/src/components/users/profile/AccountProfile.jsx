// import React, { useEffect, useState } from 'react'
// import { Box, CircularProgress, Grid } from '../../_design'
// import { useGetAccountProfile } from '../hooks'
// import { AccountSummary, AccountDetails } from './sections'

// const AccountProfile = () => {
//     const { data, loading, loaded, error } = useGetAccountProfile()
//     const [profileData, setProfileData] = useState()

//     useEffect(() => {
//         if (loaded) {
//             setProfileData(data)
//         }
//     }, [data, loaded])

//     if (error) return <div>{error}</div>

//     if (loading || !profileData)
//         return (
//             <Box display="flex" mt={10} justifyContent="center">
//                 <CircularProgress />
//             </Box>
//         )

//     return (
//         <Box mt="20px">
//             <Grid container spacing={4}>
//                 <Grid item lg={4} md={6} xl={4} xs={12}>
//                     <AccountSummary profileData={profileData} />
//                 </Grid>
//                 <Grid item lg={8} md={6} xl={8} xs={12}>
//                     <AccountDetails profileData={profileData} setProfileData={setProfileData} />
//                 </Grid>
//             </Grid>
//         </Box>
//     )
// }

// export default AccountProfile
