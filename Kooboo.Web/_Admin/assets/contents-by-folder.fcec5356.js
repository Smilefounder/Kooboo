var z=(K,i,k)=>new Promise((u,_)=>{var g=y=>{try{E(k.next(y))}catch(h){_(h)}},d=y=>{try{E(k.throw(y))}catch(h){_(h)}},E=y=>y.done?u(y.value):Promise.resolve(y.value).then(g,d);E((k=k.apply(K,i)).next())});import{_ as pe}from"./icon-button.3313e42d.js";import{_ as ue}from"./k-table.d4486972.js";import{m as fe,d as De}from"./textContent.585a78c7.js";import{a as Re}from"./content-folder.151b44e9.js";import{u as _e}from"./use-date.01b82ce0.js";import{u as Y}from"./replace-all.7cf5f327.js";import{d as ae,cd as ve,g as q,o as m,j as v,w as l,b as a,u as e,f as T,t as $,c as O,e as le,G as ye,n as he,aH as oe,x as Fe,M as Le,h as Me,N as Oe,b7 as Ue,a as ee,F as te,b6 as ne,k as J}from"./url.2e6a77c4.js";import{B as je}from"./breadcrumb.80c136b8.js";import{S as be}from"./search-input.67dfc984.js";import{b as qe}from"./confirm.e2c924ff.js";import{g as ie,v as Ae}from"./i18n.48bd28ac.js";import{u as ge}from"./main.ea42d807.js";import{u as Pe}from"./page.831d1a45.js";import{E as re,a as ze}from"./index.b494e80e.js";import{E as Je}from"./index.a439b087.js";import{E as xe}from"./index.c4e9b529.js";import{u as ke}from"./content-effect.325a9c06.js";import{_ as we}from"./index.4a51c2b7.js";import{E as Ce}from"./index.2bc50276.js";import{E as Ke}from"./windi.a5b0b048.js";import{E as Ge,a as He,c as Qe}from"./index.0d2684bf.js";import{E as We}from"./index.379e80ad.js";import"./index.8262f5ef.js";import"./index.fcc3ea42.js";import"./sortable.esm.a99254e8.js";import"./index.7034978e.js";import"./isEqual.f1ae9fb3.js";import"./_baseIsEqual.c0d7e77a.js";import"./index.fc90f5ad.js";import"./event.776e7e11.js";import"./index.ff0264f3.js";import"./error.7e8331f1.js";import"./isNil.98bb3b88.js";import"./index.77edae39.js";import"./index.695d0284.js";import"./dayjs.min.0a66969b.js";/* empty css                                                               */import"./plugin-vue_export-helper.21dcd24c.js";import"./plugin-vue_export-helper.41ffa612.js";/* empty css                                                                 */import"./logo-transparent.1566007e.js";import"./index.99c4f65d.js";import"./index.603c1365.js";import"./scroll.f51de5d8.js";import"./aria.75ec5909.js";import"./event.53b2ad83.js";import"./focus-trap.23f44899.js";import"./validator.e2869aba.js";import"./index.50c16ae5.js";import"./preload-helper.13a99eb0.js";import"./validate.efc4a5c0.js";import"./index.d75a71d9.js";import"./index.aba77680.js";import"./index.6d45a031.js";import"./index.31f26a0f.js";import"./index.60af85f4.js";import"./style.19d0c187.js";import"./toNumber.574be4f1.js";import"./index.6855d037.js";import"./_baseClone.adbc92f5.js";import"./guid.c1a40312.js";import"./debounce.61c67278.js";import"./index.064c7de9.js";import"./refs.4001ce17.js";import"./omitBy.c7280540.js";import"./pickBy.a9d2c8dd.js";import"./_basePickBy.a6ffdd2d.js";import"./isEmpty.dc565ae5.js";import"./search-provider.68dc3c2c.js";import"./last.e7aa49db.js";import"./_baseIndexOf.4d7985be.js";import"./_baseUniq.c69c2d82.js";import"./use-operation-dialog.f0f37a10.js";import"./media-list.vue_vue_type_style_index_0_scoped_true_lang.118b18f5.js";import"./file.b0d4cc6e.js";import"./index.a731e53b.js";import"./folder.8308bb9d.js";import"./relations-tag.a09361ff.js";import"./use-copy-text.a346ed23.js";import"./index.500e98ec.js";import"./browser.6147bf52.js";import"./throttle.041b2553.js";import"./_createCompounder.591d8488.js";import"./color-picker.770bc088.js";import"./index.ae716322.js";import"./position.c0f844a3.js";import"./image-cover.5aa60066.js";import"./media-dialog.00ce8f4f.js";import"./media.56507018.js";import"./index.4db7adae.js";/* empty css                                                          *//* empty css                                                          */import"./layout.7d98e9c1.js";import"./index.d971d096.js";import"./utils.6985c8b2.js";import"./index.0e262df3.js";import"./use-file-upload.82732353.js";import"./index.470f0e7e.js";import"./index.deca86b5.js";import"./index.a47bd154.js";import"./index.be352e91.js";import"./file.e38b2555.js";/* empty css                                                         */import"./image-editor.vue_vue_type_style_index_0_scoped_true_lang.f6d26366.js";import"./index.968f79fd.js";import"./dropdown.0507a1c7.js";import"./string.955a924b.js";const Xe=["onClick"],Ye=ae({setup(K,{expose:i}){const k=ve(),{onPreview:u}=Pe(),_=q(!1),{t:g}=ie(),d=q(),E=ge(),y=h=>{var V;if(!E.hasAccess("content","view"))return;const f=k.resolve(Y({name:"content",query:{id:h.id,folder:(V=d.value)==null?void 0:V.folderId}}));u(f.href)};return i({show(h){d.value=h,_.value=!0}}),(h,f)=>{const V=re,D=pe,G=ze,U=Je,H=xe;return m(),v(H,{modelValue:_.value,"onUpdate:modelValue":f[0]||(f[0]=c=>_.value=c),width:"600px","close-on-click-modal":!1,title:e(g)("common.relation"),onClosed:f[1]||(f[1]=c=>_.value=!1)},{default:l(()=>[a(U,{"max-height":"400px"},{default:l(()=>{var c;return[a(G,{data:(c=d.value)==null?void 0:c.contents,class:"el-table--gray"},{default:l(()=>[a(V,{label:e(g)("common.name")},{default:l(({row:I})=>[T("a",{class:"cursor-pointer underline","data-cy":"name",onClick:r=>e(u)(I.url)},$(I.title),9,Xe)]),_:1},8,["label"]),a(V,{label:e(g)("common.edit"),width:"80px",align:"right"},{default:l(({row:I})=>[a(D,{permission:{feature:"content",action:"view"},icon:"icon-a-writein",class:"hover:text-blue",tip:e(g)("common.edit"),"data-cy":"edit",onClick:r=>y(I)},null,8,["tip","onClick"])]),_:1},8,["label"])]),_:1},8,["data"])]}),_:1})]),_:1},8,["modelValue","title"])}}}),Ze={class:"h-60px flex items-center"},eo={class:"flex items-center"},oo=ae({props:{folder:null},emits:["reload"],setup(K,{expose:i,emit:k}){const u=K,_=q(!1),g=q(""),{t:d}=ie(),{list:E,columnLoaded:y,pagination:h,fetchList:f,columns:V,keywords:D}=ke(u.folder.id),G=O(()=>V.value.map(r=>({name:r.name,displayName:r.displayName,controlType:r.controlType,multipleValue:r.multipleValue}))),U=O(()=>E.value.map(r=>(r.$DisabledSelect=r.id===g.value,r))),H=r=>{f(1,u.folder.pageSize),g.value=r.id,_.value=!0};function c(r){return z(this,null,function*(){const b=U.value.findIndex(A=>A.id===r.id)+1,R=U.value[b],F=r,L={source:g.value,nextId:F==null?void 0:F.id,prevId:R==null?void 0:R.id,folderId:u.folder.id};yield fe(L),k("reload"),I()})}function I(){_.value=!1}return i({show:H}),(r,b)=>{const R=Ce,F=be,L=re,A=ue,Q=xe;return m(),v(Q,{modelValue:_.value,"onUpdate:modelValue":b[2]||(b[2]=p=>_.value=p),width:"800px","close-on-click-modal":!1,title:e(d)("common.move"),"custom-class":"el-dialog--fixed-footer editEmbeddedDialog","destroy-on-close":"",onClose:I},{default:l(()=>[le(a(A,{data:e(U),"row-key":"id",pagination:e(h),"max-height":300,"show-check":"","hide-delete":"","is-radio":!0,permission:{feature:"content",action:"view"},onChange:b[1]||(b[1]=p=>e(f)(p))},{leftBar:l(({selected:p})=>[T("div",Ze,[T("div",null,[a(R,{round:"",class:"dark:bg-666",disabled:!p.length,onClick:se=>c(p[0])},{default:l(()=>[T("div",eo,$(e(d)("common.moveBehind")),1)]),_:2},1032,["disabled","onClick"])])])]),bar:l(()=>[a(F,{modelValue:e(D),"onUpdate:modelValue":b[0]||(b[0]=p=>ye(D)?D.value=p:null),placeholder:e(d)("common.searchContents"),class:"w-238px",clearable:"","data-cy":"search"},null,8,["modelValue","placeholder"])]),default:l(()=>[a(we,{columns:e(G)},null,8,["columns"]),a(L,{label:e(d)("content.online"),width:"100",align:"center"},{default:l(({row:p})=>[T("span",{class:he(p.online?"text-green":"text-999"),"data-cy":"online"},$(p.online?e(d)("common.yes"):e(d)("common.no")),3)]),_:1},8,["label"]),a(L,{label:e(d)("common.lastModified"),width:"180",align:"center"},{default:l(({row:p})=>[oe($(e(_e)(p.lastModified)),1)]),_:1},8,["label"])]),_:1},8,["data","pagination"]),[[Ae,e(y)]])]),_:1},8,["modelValue","title"])}}}),to={class:"p-24"},no={class:"flex items-center py-24 justify-between"},lo={class:"max-w-300px overflow-x-clip whitespace-nowrap overflow-ellipsis"},ao={class:"space-x-16 flex items-center"},io={class:"flex"},cn=ae({setup(K){const{t:i}=ie(),k=ve(),u=Fe("folder"),{list:_,columns:g,keywords:d,currentKeyword:E,columnLoaded:y,pagination:h,fetchList:f,sortSetting:V,searchCategories:D,categoryOptions:G,onSortChanged:U}=ke(u),H=O(()=>g.value.map(o=>({name:o.name,displayName:o.displayName,controlType:o.controlType,multipleValue:o.multipleValue,attrs:{sortable:Q.value,"min-width":180}}))),c=q(),I=q(),r=Le(),b=ge(),R=q();Me(()=>z(this,null,function*(){var o;yield se(),f(1,(o=c.value)==null?void 0:o.pageSize)}));const F=O(()=>{var o,t;return((o=c.value)==null?void 0:o.sortable)&&((t=c.value)==null?void 0:t.sortField)==="dragAndDrop"}),L=O(()=>{var o;return F.value&&((o=_.value)==null?void 0:o.length)>1}),A=O(()=>L.value&&!E.value),Q=O(()=>F.value?void 0:"custom"),p=O(()=>{var o,t;return((o=c.value)==null?void 0:o.displayName)||((t=c.value)==null?void 0:t.name)||""});function se(){return z(this,null,function*(){c.value=yield Re({id:u})})}function Se(o){return z(this,null,function*(){yield qe(o.length);const t=o.map(s=>s.id);yield De({ids:t}),f(h.currentPage)})}function Ve(){var o;b.hasAccess("contentType","edit")&&k.push(Y({name:"contenttype",query:{id:(o=c.value)==null?void 0:o.contentTypeId,fromRouter:r.name,fromFolder:u}}))}function ce(o,t){k.push(Y({name:"content",query:{folder:u,id:o==null?void 0:o.id,copy:t?"true":void 0}}))}function Ie(o){!b.hasAccess("content","edit")||ce(o,!0)}function Te(o){switch(o.type){case 1:return i("common.embeddedFolder");case 2:return i("common.categoryFolder");default:return i("common.unknown")}}function Ee(o){var t;(t=R.value)==null||t.show(o)}function Be(o){var t;(t=I.value)==null||t.show(o)}function Ne(o,t){return z(this,null,function*(){const{newIndex:s,oldIndex:w}=t;if(s===void 0||w===void 0||w===s)return;const B=o[s],M=o[s+1],C=o[s-1],S={source:B.id,folderId:u,prevId:M==null?void 0:M.id,nextId:C==null?void 0:C.id};yield fe(S),yield f()})}function $e(o,t){var M,C;let s=(M=o.summaryField)!=null?M:Object.keys(o.textValues)[0];s=(C=Object.keys(o.textValues).find(S=>S.toLowerCase()==s.toLowerCase()))!=null?C:"";let w=o.textValues[s];const B=t.columns.find(S=>{var N;return((N=S.name)==null?void 0:N.toLowerCase())==(s==null?void 0:s.toLowerCase())});if(B!=null&&B.selectionOptions)try{const S=JSON.parse(B.selectionOptions);let N=[w];B.controlType=="CheckBox"&&(N=JSON.parse(w));const W=[];for(const j of N){const X=S.find(Z=>Z.value==j);X?W.push(X.key):W.push(j)}w=W.join(",")}catch(S){}return w}return(o,t)=>{var Z,me,de;const s=Ke,w=Ce,B=Ge,M=He,C=We,S=Oe("router-link"),N=pe,W=Qe,j=re,X=Ue("hasPermission");return m(),ee("div",to,[a(je,{"crumb-path":[{name:e(i)("common.contentFolders"),route:{name:"contents"}},{name:e(p)}]},null,8,["crumb-path"]),T("div",no,[le((m(),v(w,{round:"",title:e(i)("common.new")+" "+e(p),"data-cy":"new-text-content",onClick:t[0]||(t[0]=n=>ce())},{default:l(()=>[a(s,{class:"iconfont icon-a-addto"}),oe(" "+$(e(i)("common.new"))+" ",1),T("span",lo,$(e(p)),1)]),_:1},8,["title"])),[[X,{feature:"content",action:"edit"}]]),T("div",ao,[(m(!0),ee(te,null,ne(e(G),n=>{var x;return m(),v(M,{key:n.id,modelValue:e(D)[n.id],"onUpdate:modelValue":P=>e(D)[n.id]=P,clearable:"",multiple:"",placeholder:(x=n.display)!=null?x:n.alias,onChange:t[1]||(t[1]=()=>e(f)())},{default:l(()=>[(m(!0),ee(te,null,ne(n.options,P=>(m(),v(B,{key:P.id,label:$e(P,n),value:P.id},null,8,["label","value"]))),128))]),_:2},1032,["modelValue","onUpdate:modelValue","placeholder"])}),128)),a(be,{modelValue:e(d),"onUpdate:modelValue":t[2]||(t[2]=n=>ye(d)?d.value=n:null),placeholder:e(i)("common.searchContents"),class:"w-238px",clearable:"","data-cy":"search"},null,8,["modelValue","placeholder"]),e(b).hasAccess("contentType","edit")?(m(),v(S,{key:0,to:e(Y)({name:"contenttype",query:{id:(Z=c.value)==null?void 0:Z.contentTypeId,fromRouter:e(r).name,fromFolder:e(u)}})},{default:l(()=>[a(C,{class:"box-item",effect:"dark",content:e(i)("common.editContentType"),placement:"top"},{default:l(()=>[a(w,{circle:"","data-cy":"edit-content-type",onClick:Ve},{default:l(()=>[a(s,{class:"iconfont icon-a-setup"})]),_:1})]),_:1},8,["content"])]),_:1},8,["to"])):(m(),v(C,{key:1,class:"box-item",effect:"dark",content:e(i)("common.editContentType"),placement:"top"},{default:l(()=>[le((m(),v(w,{circle:"","data-cy":"edit-content-type"},{default:l(()=>[a(s,{class:"iconfont icon-a-setup"})]),_:1})),[[X,{feature:"contentType",action:"edit"}]])]),_:1},8,["content"]))])]),e(y)?(m(),v(e(ue),{key:0,data:e(_),"show-check":"",draggable:e(L),"row-key":"id",pagination:e(h),permission:{feature:"content",action:"delete"},sort:(me=e(V).prop)!=null?me:void 0,order:(de=e(V).order)!=null?de:void 0,onDelete:Se,onChange:t[3]||(t[3]=n=>e(f)(n)),onSorted:Ne,onSortChange:e(U)},{bar:l(({selected:n})=>[n.length===1?(m(),v(N,{key:0,permission:{feature:"content",action:"edit"},circle:"",class:"text-[#192845] !hover:text-blue",icon:"icon-copy",tip:e(i)("common.copy"),"data-cy":"copy",onClick:x=>Ie(n[0])},null,8,["tip","onClick"])):J("",!0),e(L)&&n.length===1?(m(),v(N,{key:1,permission:{feature:"content",action:"edit"},circle:"",class:"text-[#192845] !hover:text-blue",icon:"icon-move",tip:e(i)("common.move"),"data-cy":"move",onClick:x=>Be(n[0])},null,8,["tip","onClick"])):J("",!0)]),default:l(()=>[a(we,{columns:e(H)},null,8,["columns"]),a(j,{label:e(i)("common.usedBy"),width:"180"},{default:l(({row:n})=>[n.usedBy?(m(!0),ee(te,{key:0},ne(n.usedBy,x=>(m(),v(C,{key:x.folderId,class:"box-item",effect:"dark",content:Te(x),placement:"top"},{default:l(()=>[a(W,{class:"cursor-pointer",round:"",size:"small",title:x.displayName||x.folderName,onClick:P=>Ee(x)},{default:l(()=>[oe($(x.displayName||x.folderName),1)]),_:2},1032,["title","onClick"])]),_:2},1032,["content"]))),128)):J("",!0)]),_:1},8,["label"]),a(j,{prop:"online",sortable:e(Q),label:e(i)("content.online"),width:"120",align:"center"},{default:l(({row:n})=>[T("span",{class:he(n.online?"text-green":"text-999"),"data-cy":"online"},$(n.online?e(i)("common.yes"):e(i)("common.no")),3)]),_:1},8,["sortable","label"]),a(j,{label:e(i)("common.lastModified"),width:"180",align:"center",prop:"lastModified",sortable:e(Q)},{default:l(({row:n})=>[oe($(e(_e)(n.lastModified)),1)]),_:1},8,["label","sortable"]),a(j,{align:"right",width:e(A)?80:50},{default:l(({row:n})=>[T("div",io,[a(S,{class:"mx-8 cursor-pointer hover:text-blue",to:e(Y)({name:"content",query:{folder:e(u),id:n==null?void 0:n.id,copy:void 0}}),"data-cy":"edit"},{default:l(()=>[a(C,{class:"box-item",effect:"dark",content:e(i)("common.edit"),placement:"top"},{default:l(()=>[a(s,{class:"iconfont icon-a-writein"})]),_:1},8,["content"])]),_:2},1032,["to"]),e(A)?(m(),v(N,{key:0,icon:"icon-move js-sortable cursor-move",tip:e(i)("common.move"),"data-cy":"move"},null,8,["tip"])):J("",!0)])]),_:1},8,["width"])]),_:1},8,["data","draggable","pagination","sort","order","onSortChange"])):J("",!0),a(Ye,{ref_key:"usedByDialogRef",ref:R},null,512),c.value?(m(),v(oo,{key:1,ref_key:"moveRef",ref:I,folder:c.value,onReload:e(f)},null,8,["folder","onReload"])):J("",!0)])}}});export{cn as default};